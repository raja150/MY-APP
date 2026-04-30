import { notifyError } from 'components/alert/Toast';
import { RWDropdownList } from 'components/dynamicform/Controls';
import Declaration from 'domain/PayRoll/ITDeclaration';
import DeclarationSummary from 'domain/PayRoll/ITDeclaration/Summary';
import React, { useEffect, useState } from 'react';
import { Card, CardBody, Col, Row, Button } from 'reactstrap';
import TaxDeclarationService from 'services/PayRollServices/TaxDeclarationService';
import * as _ from 'lodash'
import PageHeader from 'Layout/AppMain/PageHeader';
import { useHistory } from 'react-router-dom';

const SelfDeclaration = (props) => {
    const [state, setState] = useState({
        isLoading: true,
        financialYears: [], selectedFYId: null, settings: {}, declaration: null, haveDeclaration: false, fromMonth: '', toMonth: ''
    });
    const [onEdit, setOnEdit] = useState(true);
    const history = useHistory();

    useEffect(() => {
        const fetchData = async () => {
            let financialYears = []
            await TaxDeclarationService.getFinancialYear().then((result) => {
                financialYears = result.data;
            })
            setState({ ...state, isLoading: false, financialYears: financialYears })
        }
        fetchData();
    }, [])

    const handleValueChange = async (name, value, { selected }) => {
        let declaration = {}, settings = {}
        let haveDeclaration = false;
        if (value) {
            await TaxDeclarationService.getMyDeclarationFY(value).then((result) => {
                if (result.data) {
                    declaration = result.data;
                    haveDeclaration = true;
                }
            }).catch(err => {
                notifyError(err.message)
            })
            if (!_.isEmpty(declaration)) {
                await TaxDeclarationService.getSettings(declaration.paySettingId).then((res) => {
                    settings = res.data
                })
            }
        }
        setState({ ...state, [name]: value, settings: settings, declaration: declaration, haveDeclaration: haveDeclaration, fromMonth: selected && selected.from, toMonth: selected && selected.to });
        setOnEdit(false);
    }

    const handleEdit = (n) => {
        setOnEdit(true)
    }
    const handleSave = (data) => {
        setState({ ...state, declaration: data, haveDeclaration: true });
        setOnEdit(false)
    }


    const handleDisplay = () => {
        //FY Selected and Declaration is not available (OR)
        //Declaration is available and on edit
        //Show Declaration edit
        if ((state.haveDeclaration === false && state.selectedFYId)
            || (state.haveDeclaration && onEdit === true)) {
            return <Declaration {...props} selectedFYId={state.selectedFYId}
                fromMonth={state.fromMonth}
                toMonth={state.toMonth}
                declaration={state.declaration}
                handelRedirect={handelRedirect}
                handleSave={handleSave} selfDeclaration={true} routeName="SelfService" />
        } //Other wise display declaration display 
        else if (state.haveDeclaration && !onEdit) {
            return <>
                <Card>
                    <CardBody>
                        <DeclarationSummary declaration={state.declaration} />

                        <Row className='justify-content-center'>
                            <Button className="mb-2 mr-2 btn-icon btn-success1" key='buttonSubmit' color="success" type="button"
                                name="submit" onClick={(e) => handleEdit()}
                                disabled={!_.isEmpty(state.settings) && state.settings.lock === 1 ? true : false}>
                                <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i>  Edit
                            </Button>
                        </Row>
                    </CardBody>
                </Card>
            </>
        }
    };

    const handelRedirect = (data) => {

        setOnEdit(false)
        setState({ ...state, declaration: data, haveDeclaration: true });
    }

    return (
        <>
            <PageHeader title='IT Declaration' />
            {
                state.isLoading ? '' :
                    <>
                        <Card className='mb-2'>
                            <CardBody>
                                <Row>
                                    <Col md='4'>
                                        <RWDropdownList {...{
                                            name: 'selectedFYId', label: 'Financial Year', valueField: 'id', textField: 'name',
                                            value: state.selectedFYId, type: 'string', values: state.financialYears,

                                        }} handlevaluechange={handleValueChange} />
                                    </Col>
                                    {/* <Col md='4' className='my-auto'>
                                        {
                                            state.haveDeclaration ? <Button className=" btn-icon btn-success" key='button' color="success" type="submit" name="save" onClick={(e) => handleEdit(state.declaration)} >
                                                <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i> {onEdit ? "Edit" : "View"}
                                            </Button> : ''
                                        }
                                    </Col> */}
                                </Row>
                            </CardBody>
                        </Card>
                        {handleDisplay()}
                    </>
            }
        </>
    );
}

export default SelfDeclaration;