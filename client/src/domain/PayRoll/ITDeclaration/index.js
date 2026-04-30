import { notifyError, notifySaved } from 'components/alert/Toast';
import { Input, Radio } from 'components/dynamicform/Controls';
import { FieldArray, Form, Formik } from 'formik';
import _ from 'lodash';
import React, { Fragment, useEffect, useState } from 'react';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import { Loader } from 'react-bootstrap-typeahead';
import 'react-table-6/react-table.css';
import { Button, Card, CardBody, CardSubtitle, CardTitle, Col, Container, Row } from 'reactstrap';
import * as Yup from 'yup';
import TaxDeclarationService from '../../../services/PayRollServices/TaxDeclarationService';
import { EightyCItem, EightyDItem, HRAEmptyItem, LetOutEmptyItem, OtherEmptyItem } from './ConstValues';
import LineItem from './HRA';
import LetOutItem from './LetOut';
import EightyC from './Section80C';
import EightyD from './Section80D';
import SectionOther from './SectionOther';
import * as formUtil from 'utils/form'
import * as compare from '../../../utils/Compare';

function Declaration(props) {

    const [state, setState] = useState({
        isLoading: true, salInfo: [], Employees: [], templateComponents: [], components: [],
        template: [], eightyCItems: [], eightyDItems: [], sectionOthers: [], declarationInfo: {}
    })
    const [isLoading, setIsLoading] = useState(true)
    useEffect(() => {
        setIsLoading(true)
        const fetchData = async () => {
            let section80cItems = [], eightyD = [], otherSections = [], declarationInfo = {}
            await TaxDeclarationService.getEightyC().then((result) => {
                section80cItems = result.data;
            })
            await TaxDeclarationService.getEightyD().then((result) => {
                eightyD = result.data;
            })
            await TaxDeclarationService.getOtherSections().then((result) => {
                otherSections = result.data;
            })

            declarationInfo = props.declaration !== null ? props.declaration : {}

            setState({
                ...state, declarationInfo: declarationInfo, eightyCItems: section80cItems,
                eightyDItems: eightyD, sectionOthers: otherSections
            })
            setIsLoading(false);
        }
        fetchData();
    }, [props.declaration])

    let formValues = {}

    if (_.isEmpty(state.declarationInfo)) {

        formValues = {
            isNewRegime: '0',
            hraItems: [HRAEmptyItem()],
            letOutItems: [LetOutEmptyItem()],
            eightyCs: [EightyCItem()],
            eightyCItems: state.eightyCItems,
            eightyDs: [EightyDItem()],
            eightyDItems: state.eightyDItems,
            sectionOtherLines: [OtherEmptyItem()],
            sectionOthers: state.sectionOthers,
            fromMonth: props.fromMonth,
            toMonth: props.toMonth,
            homeLoanPay: {
                interestPaid: 0,
                nameOfLender: '',
                lenderPAN: '',
                principle: 0
            },
            incomeSource: {
                otherSources: 0,
                interestOnSaving: 0,
                interestOnFD: 0,
            },
            prevEmployment: {
                incomeAfterException: 0,
                incomeTax: 0,
                professionalTax: 0,
                provisionalFund: 0,
                encashmentExceptions: 0
            },
        }
    }
    else {
        formValues = {
            ...state.declarationInfo, eightyCItems: state.eightyCItems,
            eightyDItems: state.eightyDItems, sectionOthers: state.sectionOthers,

        };
        formValues.isNewRegime = state.declarationInfo.isNewRegime ? '1' : '0';
        formValues.fromMonth = props.fromMonth;
        formValues.toMonth = props.toMonth;
        formValues.hraItems = [];
        formValues.letOutItems = [];
        formValues.eightyCs = [];
        formValues.eightyDs = [];
        formValues.sectionOtherLines = [];
        formValues.prevEmployment = {};

        if (state.declarationInfo.hraLines) {
            state.declarationInfo.hraLines.map(async (item, index) => {
                formValues.hraItems.push({
                    id: item.id,
                    declarationId: item.declarationId,
                    rentalFrom: item.rentalFrom,
                    rentalTo: item.rentalTo,
                    address: item.address,
                    amount: item.amount,
                    city: item.city,
                    pan: item.pan,
                    landlord: item.landlord
                })
            });
        } else {
            formValues.hraItems = [HRAEmptyItem()];
        }

        if (state.declarationInfo.homeLoanPay) {
            formValues.homeLoanPay = {
                interestPaid: state.declarationInfo.homeLoanPay.interestPaid,
                nameOfLender: state.declarationInfo.homeLoanPay.nameOfLender,
                lenderPAN: state.declarationInfo.homeLoanPay.lenderPAN,
                principle: state.declarationInfo.homeLoanPay.principle,
                id: state.declarationInfo.homeLoanPay.id,
                declarationId: state.declarationInfo.homeLoanPay.declarationId,
            }
        } else {
            formValues.homeLoanPay = {
                interestPaid: 0,
                nameOfLender: '',
                lenderPAN: '',
                principle: 0,
            }
        }

        if (state.declarationInfo.letOutPropertyLines) {
            state.declarationInfo.letOutPropertyLines.map(async (item, index) => {

                formValues.letOutItems.push({
                    annualRentReceived: item.annualRentReceived,
                    municipalTaxPaid: item.municipalTaxPaid,
                    netAnnualValue: item.netAnnualValue,
                    standardDeduction: item.standardDeduction,
                    repayingHomeLoan: item.repayingHomeLoan ? "1" : "0",
                    interestPaid: item.interestPaid,
                    nameOfLender: item.nameOfLender,
                    lenderPAN: item.lenderPAN,
                    netIncome: item.netIncome,
                    principle: item.principle,
                    id: item.id,
                    declarationId: item.declarationId,

                })
            });
        } else {
            formValues.letOutItems = [LetOutEmptyItem()];
        }

        if (state.declarationInfo.section80CLines) {
            state.declarationInfo.section80CLines.map(async (item, index) => {
                formValues.eightyCs.push({
                    section80CId: item.section80CId,
                    amount: item.amount,
                    id: item.id,
                    declarationId: item.declarationId
                })
            });
        } else {
            formValues.eightyCs = [EightyCItem()];
        }


        if (state.declarationInfo.section80DLines) {
            state.declarationInfo.section80DLines.map(async (item, index) => {
                formValues.eightyDs.push({
                    section80DId: item.section80DId,
                    amount: item.amount,
                    id: item.id,
                    declarationId: item.declarationId,
                    limit: item.limit
                })
            });
        } else {
            formValues.eightyDs = [EightyDItem()];
        }


        if (state.declarationInfo.sectionOtherLines) {
            state.declarationInfo.sectionOtherLines.map(async (item, index) => {
                formValues.sectionOtherLines.push({
                    otherSectionsId: item.otherSectionsId,
                    amount: item.amount,
                    id: item.item,
                    declarationId: item.declarationId,
                })
            });
        } else {
            formValues.sectionOtherLines = [OtherEmptyItem()];
        }

        if (state.declarationInfo.incomeSource) {
            formValues.incomeSource = {
                otherSources: state.declarationInfo.incomeSource.otherSources,
                interestOnSaving: state.declarationInfo.incomeSource.interestOnSaving,
                interestOnFD: state.declarationInfo.incomeSource.interestOnFD,
                id: state.declarationInfo.incomeSource.id,
                declarationId: state.declarationInfo.incomeSource.declarationId
            }
        } else {
            formValues.incomeSource = {
                otherSources: 0,
                interestOnSaving: 0,
                interestOnFD: 0,
            }
        }

        if (!_.isEmpty(state.declarationInfo.prevEmployment)) {
            formValues.prevEmployment = {
                incomeAfterException: state.declarationInfo.prevEmployment.incomeAfterException,
                incomeTax: state.declarationInfo.prevEmployment.incomeTax,
                professionalTax: state.declarationInfo.prevEmployment.professionalTax,
                provisionalFund: state.declarationInfo.prevEmployment.provisionalFund,
                encashmentExceptions: state.declarationInfo.prevEmployment.encashmentExceptions,
                id: state.declarationInfo.prevEmployment.id,
                declarationId: state.declarationInfo.prevEmployment.declarationId,
            }
        }
    }
    const handleSave = async (values, actions) => {
        let hraLines = [], letOutPropertyLines = [], eightyCLines = [],
            eightyDLines = [], sectionOtherLines = []
        const declarationId = state.declarationInfo && state.declarationInfo.id ? state.declarationInfo.id : '';
        values.hraItems.length > 0 && values.hraItems.map((houseRent, i) => {
            let obj = {}
            if (houseRent.rentalFrom != '' && houseRent.rentalTo != '') {
                obj["rentalFrom"] = houseRent.rentalFrom
                obj["rentalTo"] = houseRent.rentalTo
                obj["amount"] = houseRent.amount
                obj["address"] = houseRent.address
                obj["city"] = houseRent.city
                obj["pan"] = houseRent.pan
                obj["landlord"] = houseRent.landlord
            }
            if (houseRent.id) { obj["id"] = houseRent.id; }
            if (!_.isEmpty(declarationId)) { obj["declarationId"] = declarationId }
            if (!_.isEmpty(obj)) {
                hraLines.push(obj)
            }
        })

        values.letOutItems.length > 0 && values.letOutItems.map((houseRent, i) => {
            let obj = {}
            if (houseRent.annualRentReceived != 0 && houseRent.municipalTaxPaid != 0) {
                obj["annualRentReceived"] = houseRent.annualRentReceived
                obj["municipalTaxPaid"] = houseRent.municipalTaxPaid
                obj["netAnnualValue"] = houseRent.netAnnualValue
                obj["standardDeduction"] = houseRent.standardDeduction
                obj["repayingHomeLoan"] = houseRent.repayingHomeLoan == "1" ? true : false
                obj["interestPaid"] = houseRent.interestPaid
                obj["principle"] = houseRent.principle
                obj["nameOfLender"] = houseRent.nameOfLender
                obj["lenderPAN"] = houseRent.lenderPAN
                obj["netIncome"] = houseRent.netIncome
                obj["principle"] = houseRent.principle
            }
            if (houseRent.id) {
                obj["id"] = houseRent.id;
            }
            if (!_.isEmpty(declarationId)) {
                obj["declarationId"] = declarationId
            }
            if (!_.isEmpty(obj)) {
                letOutPropertyLines.push(obj)
            }
        })
        values.eightyCs.length > 0 && values.eightyCs.map((itemEighty, i) => {
            let obj = {}
            if (!_.isEmpty(itemEighty.section80CId)) {
                obj["section80CId"] = itemEighty.section80CId
                obj["amount"] = itemEighty.amount

                if (itemEighty.id) { obj["id"] = itemEighty.id; }
                if (!_.isEmpty(declarationId)) { obj["declarationId"] = declarationId }
                eightyCLines.push(obj)
            }
        })


        values.eightyDs.length > 0 && values.eightyDs.map((itemEighty, i) => {
            let obj = {}
            if (!_.isEmpty(itemEighty.section80DId)) {
                obj["section80DId"] = itemEighty.section80DId
                obj["amount"] = itemEighty.amount
                if (itemEighty.id) { obj["id"] = itemEighty.id; }
                if (!_.isEmpty(declarationId)) { obj["declarationId"] = declarationId }
                eightyDLines.push(obj)
            }
        })

        values.sectionOtherLines.length > 0 && values.sectionOtherLines.map((itemOther, i) => {
            let obj = {}
            if (!_.isEmpty(itemOther.otherSectionsId)) {
                obj['otherSectionsId'] = itemOther.otherSectionsId;
                obj['amount'] = itemOther.amount

                if (itemOther.id) { obj["id"] = itemOther.id; }
                if (!_.isEmpty(declarationId)) { obj["declarationId"] = declarationId }

                sectionOtherLines.push(obj)
            }
        })

        const data = {
            FinancialYearId: props.financialYearId,
            isNewRegime: compare.isEqual(values.isNewRegime, '1'),
            employeeId: props.employeeID ? props.employeeID : '',
            HRALines: hraLines,
            LetOutPropertyLines: letOutPropertyLines,
            Section80CLines: eightyCLines,
            Section80DLines: eightyDLines,
            SectionOtherLines: sectionOtherLines,
        }
        if (values.homeLoanPay && values.homeLoanPay.interestPaid > 0) {

            data.homeLoanPay = {
                interestPaid: values.homeLoanPay.interestPaid,
                nameOfLender: values.homeLoanPay.nameOfLender,
                lenderPAN: values.homeLoanPay.lenderPAN,
                principle: values.homeLoanPay.principle,
            };
            if (values.homeLoanPay.id) { data.homeLoanPay.id = values.homeLoanPay.id; }
            if (!_.isEmpty(declarationId)) { data.homeLoanPay.declarationId = declarationId }

        }
        if (values.incomeSource && (values.incomeSource.otherSources > 0
            || values.incomeSource.interestOnSaving > 0
            || values.incomeSource.interestOnFD > 0)) {
            data.incomeSource = {
                otherSources: values.incomeSource.otherSources,
                interestOnSaving: values.incomeSource.interestOnSaving,
                interestOnFD: values.incomeSource.interestOnFD
            }
            if (values.incomeSource.id) { data.incomeSource.id = values.incomeSource.id; }
            if (!_.isEmpty(declarationId)) { data.incomeSource.declarationId = declarationId }
        }
        if (values.prevEmployment && values.prevEmployment.incomeAfterException > 0) {
            data.prevEmployment = {
                incomeAfterException: values.prevEmployment.incomeAfterException,
                incomeTax: values.prevEmployment.incomeTax,
                professionalTax: values.prevEmployment.professionalTax,
                provisionalFund: values.prevEmployment.provisionalFund,
                encashmentExceptions: values.prevEmployment.encashmentExceptions
            };

            if (values.prevEmployment.id) { data.prevEmployment.id = values.prevEmployment.id; }
            if (!_.isEmpty(declarationId)) { data.prevEmployment.declarationId = declarationId }

        }
        if (_.isEmpty(data['financialYearId'])) {
            data['financialYearId'] = props.selectedFYId;
        }

        if (!_.isEmpty(state.declarationInfo)) {
            data['employeeId'] = state.declarationInfo.employeeId
            data['id'] = state.declarationInfo.id
        } else if (props.employeeID) {
            data['employeeId'] = props.employeeID
        }

        await TaxDeclarationService.postMyDeclaration(data, props.selfDeclaration).then((result) => {

            if (result.status == 200) {
                notifySaved("Data saved successfully")
                props.handelRedirect(result.data);
            }
        }).catch(error => {
            formUtil.displayFormikError(error, actions)
            // notifyError(error.message)
        })


    }

    const validationSchema = Yup.object({
        hraItems: Yup.array()
            .of(Yup.object().shape({
                // rentalFrom: Yup.string().required("Rental from is required"),
                //rentalTo: Yup.string().required("Rental To is Required"),
                rentalTo: Yup.string().when('rentalFrom', {
                    is: (val) => val !== undefined,
                    then: Yup.string().required("This field is required!")
                }),
                amount: Yup.number().when('rentalFrom', {
                    is: (val) => val !== undefined,
                    then: Yup.number().required("This field is required! ").min(1, 'This field is required!'),
                    otherwise: Yup.number().nullable()
                }),
                // rentalTo: Yup.string().when('rentalFrom', {
                //     is: (val) => val !== undefined,
                //     then: Yup.string().required("This field is required!")
                // }),
                // pan: Yup.string().when('rentalFrom', {
                //     is: (val) => val !== undefined,
                //     then: Yup.string().required("This field is required!")
                // }),
                city: Yup.string().when('rentalFrom', {
                    is: (val) => val !== undefined,
                    then: Yup.string().required("This field is required!")
                }),
                address: Yup.string().when('rentalFrom', {
                    is: (val) => val !== undefined,
                    then: Yup.string().required("This field is required!")
                }),
                pan: Yup.string().when('amount', {
                    is: (val) => val >= 10000,
                    then: Yup.string().required("This field is required!")
                }),
                landlord: Yup.string().when('amount', {
                    is: (val) => val >= 10000,
                    then: Yup.string().required("This field is required!")
                })

            })
            ),
        homeLoanPay: Yup.object().shape({
            nameOfLender: Yup.string().when('interestPaid', {
                is: (val) => val > 0,
                then: Yup.string().required("This field is required! ")
            }),
            lenderPAN: Yup.string().when('interestPaid', {
                is: (val) => val > 0,
                then: Yup.string().required("This field is required! ")
            }),
            interestPaid: Yup.number().min(0, 'Must be Greater than 0'),
            principle: Yup.number().when('interestPaid', {
                is: (val) => val > 0,
                then: Yup.number().required("This field is required! ").min(1, 'Must be Greater than 0'),
                otherwise: Yup.number().nullable()
            }),
        }),

        letOutItems: Yup.array().of(Yup.object().shape({

            municipalTaxPaid: Yup.number().when('annualRentReceived', {
                is: (val) => val > 0,
                then: Yup.number().required("This field is required! ").min(1, 'Must be greater than 0'),
            }),
            netAnnualValue: Yup.number().when('annualRentReceived', {
                is: (val) => val > 0,
                then: Yup.number().required("This field is required! ").min(1)
            }),
            standardDeduction: Yup.number().when('annualRentReceived', {
                is: (val) => val > 0,
                then: Yup.number().required("This field is required! ").min(1)
            }),

            interestPaid: Yup.number().when('repayingHomeLoan', {
                is: (val) => val == "1",
                then: Yup.number().required("This field is required! ").min(1, 'Must be greater than 0'),
                otherwise: Yup.number().nullable()
            }),
            principle: Yup.number().when('repayingHomeLoan', {
                is: (val) => val == "1",
                then: Yup.number().required("This field is required! ").min(1, 'Must be greater than 0'),
                otherwise: Yup.number().nullable()
            }),
            nameOfLender: Yup.string().when('repayingHomeLoan', {
                is: (val) => val == "1",
                then: Yup.string().required("This field is required! "),
                otherwise: Yup.string().nullable()
            }).nullable(),
            lenderPAN: Yup.string().when('repayingHomeLoan', {
                is: (val) => val == "1",
                then: Yup.string().required("This field is required! ").typeError('This field is required'),
                otherwise: Yup.string().nullable()
            }),
        })),

        eightyCs: Yup.array().of(Yup.object().shape({
            amount: Yup.number().when('section80CId', {
                is: (val) => val !== undefined,
                then: Yup.number().required('This value is required!').min(1, 'This value is required!')
            })
        })),

        eightyDs: Yup.array().of(Yup.object().shape({
            amount: Yup.number().when('section80DId', {
                is: (val) => val !== undefined,
                then: Yup.number().required('This value is required!').min(1, 'This value is required!')
            }),
        })),

        sectionOtherLines: Yup.array().of(Yup.object().shape({
            amount: Yup.number().when('otherSectionsId', {
                is: (val) => val !== undefined,
                then: Yup.number().required().min(1, 'This value is required!')
            })
        })),
        incomeSource: Yup.object().shape({
            interestOnFD: Yup.number().when('otherSources', {
                is: (val) => val > 0,
                then: Yup.number().required("This field is required! ").min(1, 'Must be Greater than 0'),
                otherwise: Yup.number().nullable()
            }),
            interestOnSaving: Yup.number().when('otherSources', {
                is: (val) => val > 0,
                then: Yup.number().required("This field is required! ").min(1, 'Must be Greater than 0'),
                otherwise: Yup.number().nullable()
            }),
        }),
        prevEmployment: Yup.object().shape({
            incomeTax: Yup.number().when('incomeAfterException', {
                is: (val) => val > 0,
                then: Yup.number().required("This field is required! ").min(1, 'Must be Greater than 0'),
                otherwise: Yup.number().nullable()
            }),
            professionalTax: Yup.number().when('incomeAfterException', {
                is: (val) => val > 0,
                then: Yup.number().required("This field is required! ").min(1, 'Must be Greater than 0'),
                otherwise: Yup.number().nullable()
            }),
            provisionalFund: Yup.number().when('incomeAfterException', {
                is: (val) => val > 0,
                then: Yup.number().required("This field is required! ").min(1, 'Must be Greater than 0'),
                otherwise: Yup.number().nullable()
            }),
            encashmentExceptions: Yup.number().when('incomeAfterException', {
                is: (val) => val > 0,
                then: Yup.number().required("This field is required! ").min(1, 'Must be Greater than 0'),
                otherwise: Yup.number().nullable()
            }),

        })

    })

    return (
        isLoading ? <Loader type="ball-grid-pulse" /> :
            <Fragment>
                <ReactCSSTransitionGroup
                    component="div"
                    transitionName="TabsAnimation"
                    transitionAppear={true}
                    transitionAppearTimeout={0}
                    transitionEnter={false}
                    transitionLeave={false}
                    key="roles">
                    <Container fluid>
                        <Formik
                            initialValues={formValues}
                            validationSchema={validationSchema}
                            onSubmit={(values, actions) => handleSave(values, actions)}
                        >
                            {({ values, errors, touched, setFieldValue, actions, isSubmitting, handleChange }) => {

                                const handleValueChange = async (name, value, { selected }) => {
                                    setFieldValue(name, value)
                                };
                                return (
                                    <Form>
                                        <div className="app-page-title pt-2 pb-2">
                                            <div className="page-title-wrapper">
                                                <div className="page-title-heading">
                                                    <div>Your IT Declaration</div>
                                                </div>
                                            </div>
                                        </div>
                                        <Radio {...{
                                            name: 'isNewRegime', label: '',
                                            values: [{ text: 'Old Regime', value: 0 }, { text: 'New Regime', value: 1 },], value: values['isNewRegime']
                                        }} handlevaluechange={handleValueChange}
                                        />

                                        {compare.isEqual(values.isNewRegime, '0') ?
                                            <>
                                                <Card className="mb-3">
                                                    <CardBody>
                                                        <Row>
                                                            <Col md='12' >
                                                                <CardTitle>House Rent Details</CardTitle>
                                                            </Col>
                                                        </Row>
                                                        <FieldArray
                                                            name="hraItems"
                                                            component={LineItem}
                                                        />
                                                    </CardBody>
                                                </Card>
                                                <Card className="mb-3">
                                                    <CardBody>
                                                        <Row>
                                                            <Col md='12' >
                                                                <CardTitle>Repaying Home Loan</CardTitle>
                                                            </Col>
                                                        </Row>
                                                        <Row>
                                                            <Col md='6'>
                                                                <Input
                                                                    {...{
                                                                        name: 'homeLoanPay.interestPaid',
                                                                        label: 'Interest Paid On Home Loan',
                                                                        value: values.homeLoanPay['interestPaid'],
                                                                        type: 'number',
                                                                        disabled: false,
                                                                        error: !_.isEmpty(errors) && errors.homeLoanPay && errors.homeLoanPay['interestPaid'],
                                                                        touched: !_.isEmpty(touched) && touched.homeLoanPay && touched.homeLoanPay['interestPaid'],
                                                                    }}
                                                                    handlevaluechange={handleValueChange}
                                                                />
                                                            </Col>
                                                            <Col md='6'>
                                                                <Input
                                                                    {...{
                                                                        name: 'homeLoanPay.principle',
                                                                        label: 'Principle of Home Loan',
                                                                        value: values.homeLoanPay['principle'],
                                                                        type: 'number',
                                                                        disabled: false,
                                                                        error: !_.isEmpty(errors) && errors.homeLoanPay && errors.homeLoanPay['principle'],
                                                                        touched: !_.isEmpty(touched) && touched.homeLoanPay && touched.homeLoanPay['principle'],
                                                                    }}
                                                                    handlevaluechange={handleValueChange}
                                                                />
                                                            </Col>
                                                            <Col md='6'>
                                                                <Input
                                                                    {...{
                                                                        name: 'homeLoanPay.nameOfLender',
                                                                        label: 'Name Of The Lender',
                                                                        value: values.homeLoanPay['nameOfLender'],
                                                                        type: 'string',
                                                                        disabled: false,
                                                                        error: !_.isEmpty(errors) && errors.homeLoanPay && errors.homeLoanPay['nameOfLender'],
                                                                        touched: !_.isEmpty(touched) && touched.homeLoanPay && touched.homeLoanPay['nameOfLender'],
                                                                    }}
                                                                    handlevaluechange={handleValueChange}
                                                                />
                                                            </Col>
                                                            <Col md='6'>
                                                                <Input
                                                                    {...{
                                                                        name: 'homeLoanPay.lenderPAN',
                                                                        label: 'PAN No',
                                                                        value: values.homeLoanPay['lenderPAN'],
                                                                        type: 'string',
                                                                        disabled: false,
                                                                        error: !_.isEmpty(errors) && errors.homeLoanPay && errors.homeLoanPay['lenderPAN'],
                                                                        touched: !_.isEmpty(touched) && touched.homeLoanPay && touched.homeLoanPay['lenderPAN'],
                                                                    }}
                                                                    handlevaluechange={handleValueChange}
                                                                />
                                                            </Col>

                                                        </Row>
                                                    </CardBody>
                                                </Card>
                                                <Card className="mb-3">
                                                    <CardBody>
                                                        <Row>
                                                            <Col md='12' >
                                                                <CardTitle>Receiving Rental Income From Let-out Property</CardTitle>
                                                            </Col>
                                                        </Row>
                                                        <Row>
                                                            <Col md='12'>
                                                                <FieldArray
                                                                    name="letOutItems"
                                                                    component={LetOutItem}
                                                                />
                                                            </Col>
                                                        </Row>
                                                    </CardBody>
                                                </Card>
                                                <Card className="mb-3">
                                                    <CardBody>
                                                        <Row>
                                                            <Col md='12' >
                                                                <CardTitle>Section 80C Investments</CardTitle>
                                                            </Col>
                                                        </Row>
                                                        <Row>
                                                            <Col md='12' >
                                                                <CardSubtitle className="mb-2 text-muted">Max Limit Of This Section Is :
                                                                    {/* {state.sectionITDeclarations.length > 0 ? state.sectionITDeclarations[0].maxLimitEightyC : ''}  */}
                                                                </CardSubtitle>
                                                            </Col>
                                                        </Row>
                                                        <Row>
                                                            <Col md='12'>
                                                                <FieldArray
                                                                    name="eightyCs"
                                                                    component={EightyC}
                                                                />
                                                            </Col>
                                                        </Row>
                                                    </CardBody>
                                                </Card>
                                                <Card className="mb-3">
                                                    <CardBody>
                                                        <Row>
                                                            <Col md='12' >
                                                                <CardTitle>Section 80D Investments</CardTitle>
                                                            </Col>
                                                        </Row>
                                                        <Row>
                                                            <Col md='12' >
                                                                <CardSubtitle className="mb-2 text-muted">Max Limit Of This Section Is :
                                                                    {/* {state.sectionITDeclarations.length > 0 ? state.sectionITDeclarations[0].maxLimitEightyD : ''}   */}
                                                                </CardSubtitle>
                                                            </Col>
                                                        </Row>
                                                        <Row>
                                                            <Col md='12'>
                                                                <FieldArray
                                                                    name="eightyDs"
                                                                    component={EightyD}
                                                                />
                                                            </Col>
                                                        </Row>
                                                    </CardBody>
                                                </Card>
                                                <Card className="mb-3">
                                                    <CardBody>
                                                        <Row>
                                                            <Col md='12' >
                                                                <CardTitle>Other Section Investments</CardTitle>
                                                            </Col>
                                                        </Row>
                                                        <Row>
                                                            <Col md='12'>
                                                                <FieldArray
                                                                    name="sectionOtherLines"
                                                                    component={SectionOther}
                                                                />
                                                            </Col>
                                                        </Row>
                                                    </CardBody>
                                                </Card>
                                                <Card className="mb-3">
                                                    <CardBody>
                                                        <Row>
                                                            <Col md='12' >
                                                                <CardTitle>Other Sources Of Income</CardTitle>
                                                            </Col>
                                                        </Row>
                                                        <Row>
                                                            <Col md='6'>
                                                                <Input
                                                                    {...{
                                                                        name: 'incomeSource.otherSources',
                                                                        label: 'Income From Other Sources',
                                                                        value: values.incomeSource['otherSources'],
                                                                        type: 'number',
                                                                        disabled: false,
                                                                        error: !_.isEmpty(errors) && errors.incomeSource && errors.incomeSource['otherSources'],
                                                                        touched: !_.isEmpty(touched) && touched.incomeSource && touched.incomeSource['otherSources'],
                                                                        // error: errors['otherSources'],
                                                                        // touched: touched['otherSources'],
                                                                    }}
                                                                    handlevaluechange={handleValueChange} />
                                                            </Col>
                                                            <Col md='6'>
                                                                <Input
                                                                    {...{
                                                                        name: 'incomeSource.interestOnSaving',
                                                                        label: 'Interest Earned From Savings Deposit',
                                                                        value: values.incomeSource['interestOnSaving'],
                                                                        type: 'string',
                                                                        disabled: false,
                                                                        error: !_.isEmpty(errors) && errors.incomeSource && errors.incomeSource['interestOnSaving'],
                                                                        touched: !_.isEmpty(touched) && touched.incomeSource && touched.incomeSource['interestOnSaving'],
                                                                        // error: errors['interestOnSaving'],
                                                                        // touched: touched['interestOnSaving'],
                                                                    }}
                                                                    handlevaluechange={handleValueChange} />
                                                            </Col>
                                                            <Col md='6'>
                                                                <Input
                                                                    {...{
                                                                        name: 'incomeSource.interestOnFD',
                                                                        label: 'Interest Earned From Fixed Deposit',
                                                                        value: values.incomeSource['interestOnFD'],
                                                                        type: 'string',
                                                                        disabled: false,
                                                                        error: !_.isEmpty(errors) && errors.incomeSource && errors.incomeSource['interestOnFD'],
                                                                        touched: !_.isEmpty(touched) && touched.incomeSource && touched.incomeSource['interestOnFD'],
                                                                        // error: errors['interestOnFD'],
                                                                        // touched: touched['interestOnFD'],
                                                                    }}
                                                                    handlevaluechange={handleValueChange} />
                                                            </Col>
                                                        </Row>
                                                    </CardBody>
                                                </Card>
                                                <Card className="mb-3">
                                                    <CardBody>
                                                        <Row>
                                                            <Col md='12' >
                                                                <CardTitle>Previous Employment</CardTitle>
                                                            </Col>
                                                        </Row>
                                                        <Row>
                                                            <Col md='6'>
                                                                <Input
                                                                    {...{
                                                                        name: 'prevEmployment.incomeAfterException',
                                                                        label: 'Income After Exception',
                                                                        value: values.prevEmployment['incomeAfterException'],
                                                                        type: 'number',
                                                                        disabled: false,
                                                                        error: !_.isEmpty(errors) && errors.prevEmployment && errors.prevEmployment['incomeAfterException'],
                                                                        touched: !_.isEmpty(touched) && touched.prevEmployment && touched.prevEmployment['incomeAfterException'],
                                                                        // error: errors['incomeAfterException'],
                                                                        // touched: touched['incomeAfterException'],
                                                                    }}
                                                                    handlevaluechange={handleValueChange}
                                                                />
                                                            </Col>
                                                            <Col md='6'>
                                                                <Input
                                                                    {...{
                                                                        name: 'prevEmployment.incomeTax',
                                                                        label: 'Income Tax',
                                                                        value: values.prevEmployment['incomeTax'],
                                                                        type: 'string',
                                                                        disabled: false,
                                                                        error: !_.isEmpty(errors) && errors.prevEmployment && errors.prevEmployment['incomeTax'],
                                                                        touched: !_.isEmpty(touched) && touched.prevEmployment && touched.prevEmployment['incomeTax'],
                                                                        // error: errors['incomeTax'],
                                                                        // touched: touched['incomeTax'],
                                                                    }}
                                                                    handlevaluechange={handleValueChange}
                                                                />
                                                            </Col>
                                                            <Col md='6'>
                                                                <Input
                                                                    {...{
                                                                        name: 'prevEmployment.professionalTax',
                                                                        label: 'Professional Tax',
                                                                        value: values.prevEmployment['professionalTax'],
                                                                        type: 'string',
                                                                        disabled: false,
                                                                        error: !_.isEmpty(errors) && errors.prevEmployment && errors.prevEmployment['professionalTax'],
                                                                        touched: !_.isEmpty(touched) && touched.prevEmployment && touched.prevEmployment['professionalTax'],
                                                                        // error: errors['professionalTax'],
                                                                        // touched: touched['professionalTax'],
                                                                    }}
                                                                    handlevaluechange={handleValueChange}
                                                                />
                                                            </Col>
                                                            <Col md='6'>
                                                                <Input
                                                                    {...{
                                                                        name: 'prevEmployment.provisionalFund',
                                                                        label: 'Employee Provisional Fund',
                                                                        value: values.prevEmployment['provisionalFund'],
                                                                        type: 'string',
                                                                        disabled: false,
                                                                        error: !_.isEmpty(errors) && errors.prevEmployment && errors.prevEmployment['provisionalFund'],
                                                                        touched: !_.isEmpty(touched) && touched.prevEmployment && touched.prevEmployment['provisionalFund'],
                                                                        // error: errors['provisionalFund'],
                                                                        // touched: touched['provisionalFund'],
                                                                    }}
                                                                    handlevaluechange={handleValueChange}
                                                                />
                                                            </Col>
                                                            <Col md='6'>
                                                                <Input
                                                                    {...{
                                                                        name: 'prevEmployment.encashmentExceptions',
                                                                        label: 'Leave Encashment Exceptions',
                                                                        value: values.prevEmployment['encashmentExceptions'],
                                                                        type: 'string',
                                                                        disabled: false,
                                                                        error: !_.isEmpty(errors) && errors.prevEmployment && errors.prevEmployment['encashmentExceptions'],
                                                                        touched: !_.isEmpty(touched) && touched.prevEmployment && touched.prevEmployment['encashmentExceptions'],
                                                                        // error: errors['encashmentExceptions'],
                                                                        // touched: touched['encashmentExceptions'],
                                                                    }}
                                                                    handlevaluechange={handleValueChange}
                                                                />
                                                            </Col>
                                                        </Row>
                                                    </CardBody>
                                                </Card>
                                            </>
                                            : ''}
                                        <Row className='justify-content-center'>
                                            <Col md='2' className='mb-1'>
                                                <Button className="mb-2 mr-2 btn-icon btn-success" key='button' color="success" type="submit" name="save" disabled={isSubmitting} >
                                                    <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i> {"Save"}
                                                </Button>
                                            </Col>
                                        </Row>
                                    </Form>

                                );
                            }}
                        </Formik>
                    </Container>
                </ReactCSSTransitionGroup>
            </Fragment>
    );
}
export default Declaration;