import { Input, Radio, RWDropdownList, TextAreaInput } from 'components/dynamicform/Controls';
import { Form, Formik } from 'formik';
import React, { Fragment, useEffect, useState } from 'react';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import { Loader } from 'react-bootstrap-typeahead';
import { Slide, toast } from 'react-toastify';
import { Button, Card, CardBody, Col, Container, Row } from 'reactstrap';
import * as Yup from 'yup';
import ApprovalCompensatoryService from '../../../services/Approval/CompensatoryDayService';
import WFHService from '../../../services/Approval/WFHService';
import * as formUtil from 'utils/form'
import { notifySaved } from 'components/alert/Toast';
import SessionStorageService from 'services/SessionStorage'
function CompensatoryWork(props) {
    const [isLoading, setIsLoading] = useState(false)
    const [state, setState] = useState({ entityValues: {}, employees: [], reporting: [] });
    // const parsed = queryString.parse(props.location.search);
    // const rid = parsed.r ? crypto.decrypt(parsed.r) : null;
    const rid = props.rid ? props.rid : null;

    useEffect(() => {
        setIsLoading(true)
        const fetchData = async () => {
            let entityValues = {}, employees = [], reporting = {}

            await ApprovalCompensatoryService.GetEmployees().then((result) => {
                reporting = result.data;
            })
            if (rid) {

                // await ApprovalCompensatoryService.GetApplyCompensatoryById(userId).then((result) => {

                //     entityValues = result.data;

                // }).catch((error) => {
                //     notifyError(error.message);
                // });

                await WFHService.getEmp(props.state.employeeId).then((result) => {
                    employees = result.data;
                    // reporting.reportingToId = employees.reportingToId;
                })

                // await 
            }
            setState({ ...state, entityValues: entityValues, employees: employees, reporting: reporting })
            setIsLoading(false);
        }
        fetchData();
    }, [])
    let formValues = {
        name: state.employees.name,
        approvedById: state.employees.reportingToId,
        approve: '',
        reason: ''
    }


    const handleSave = async (values, actions) => {
        const data = {

            employeeId: props.state.employeeId,
            ToDate: props.state.toDate,
            FromDate: props.state.fromDate,
            to: props.state.to,
            approvedById: values.approvedById,
            from: props.state.from,
            shiftId: props.state.shiftId,
            emailID: props.state.emailID,
            approvedById: values.approvedById,
            status: values.approve,
            adminReason: values.reason,
            reasonForApply: props.state.reasonForApply
        }
        if (rid) {
            data['id'] = rid
        }
        await ApprovalCompensatoryService.updateAsync(data).then((res) => {
            notifySaved();
        }).catch((er) => {
            formUtil.displayFormikError(er, actions)
        })
    }
    const validationSchema = Yup.object({
        employeeId: Yup.string().required(),
        approvedById: Yup.string().required(),
        approve: Yup.string().required(),
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
                    key="roles"
                >
                    <Container fluid>
                        <Formik
                            initialValues={formValues}
                            //validationSchema={validationSchema}
                            onSubmit={(values, actions) => handleSave(values, actions)}
                        >
                            {({ values, errors, touched, setFieldValue, actions }) => {
                                const handleValueChange = async (name, value, { selected }) => {
                                    setFieldValue(name, value)
                                };
                                return (
                                    <Form>
                                        <Card className="mb-3">
                                            <CardBody>
                                                <Row>
                                                    <Col xs='6'>
                                                        {/* <RWDropdownList
                                                            {...{
                                                                name: 'empId',
                                                                label: 'Employee',
                                                                valueField: 'empid',
                                                                textField: 'name',
                                                                value: values['empId'],
                                                                //values: state.employees,
                                                                error: errors['empId'],
                                                                touched: touched['empId'],
                                                            }}
                                                            handlevaluechange={handleValueChange}
                                                        /> */}
                                                        <Input
                                                            {...{
                                                                name: 'name',
                                                                label: 'Employee',
                                                                value: values['name'],
                                                                type: 'string',
                                                                disabled: true,
                                                                error: errors['name'],
                                                                touched: touched['name'],
                                                            }}
                                                            handlevaluechange={handleValueChange}
                                                        />
                                                    </Col >
                                                    <Col xs='6'>
                                                        <RWDropdownList
                                                            {...{
                                                                name: 'approvedById',
                                                                label: 'Approved By',
                                                                valueField: 'id',
                                                                textField: 'name',
                                                                value: values['approvedById'],
                                                                values: state.reporting,
                                                                error: errors['approvedById'],
                                                                touched: touched['approvedById'],
                                                            }}
                                                            handlevaluechange={handleValueChange}
                                                        />
                                                    </Col>
                                                    <Col xs='6'>
                                                        <Radio {...{
                                                            name: 'approve', label: 'Do You Want To Approve', disabled: false,
                                                            value: values['approve'],
                                                            values: [{ value: 2, text: 'Yes' }, { value: 3, text: 'No' }],
                                                            touched: touched['approve'],
                                                            error: errors['approve']
                                                        }} handlevaluechange={handleValueChange}
                                                        />
                                                    </Col>
                                                    {values.approve == 3 ?
                                                        <Col md='6'>
                                                            <TextAreaInput {...{
                                                                name: 'reason', label: 'reason', value: values['reason'], values: ['reason'],
                                                                error: errors['reason'], touched: touched['reason']
                                                            }} handlevaluechange={handleValueChange}
                                                            />
                                                        </Col> : ''
                                                    }
                                                </Row>
                                            </CardBody>
                                        </Card>
                                        <Row className='justify-content-center'>
                                            <Button className="mb-2 mr-2 btn-icon btn-success" key='button' color="success" type="submit" name="save" >
                                                <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i> {"Save"}
                                            </Button>
                                        </Row>
                                    </Form>
                                )
                            }}
                        </Formik>
                    </Container>
                </ReactCSSTransitionGroup>
            </Fragment>
    )
}
export default CompensatoryWork