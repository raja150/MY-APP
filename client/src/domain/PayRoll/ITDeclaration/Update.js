import { notifyError } from 'components/alert/Toast';
import queryString from 'query-string';
import React, { Fragment, useEffect, useState } from 'react';
import { Card, CardBody, Col, Label, Row, Button } from 'reactstrap';
import TaxDeclarationService from '../../../services/PayRollServices/TaxDeclarationService';
import * as crypto from '../../../utils/Crypto';
import Declaration from './index';
import DeclarationSummary from './Summary';
import * as _ from 'lodash';
import { useHistory } from 'react-router-dom';

const DeclarationUpdate = (props) => {
    const [state, setState] = useState({ isLoading: true, financialYears: [], financialYear: {}, declaration: {}, declarationInfo: {}, settings: {} })
    const [onEdit, setOnEdit] = useState(true);
    const history = useHistory();
    const parsed = queryString.parse(props.location.search);
    const rid = parsed.r ? crypto.decrypt(parsed.r) : null;

    useEffect(() => {
        const fetchData = async () => {
            let financialYears = [], declaration = '', financialYear = {}, settings = {}

            await TaxDeclarationService.getFinancialYear().then((result) => {
                financialYears = result.data;
            });

            if (rid) {

                // await TaxDeclarationService.getPayrollDeclaration(rid).then((result) => {
                //     declaration = result.data;
                // }).catch(err => {
                //     notifyError(err.message)
                // })

                await TaxDeclarationService.getEmpTaxDeclarationInfo(rid).then((result) => {
                    declaration = result.data;
                    financialYear = financialYears.find((e) => e.id === result.data.financialYearId)
                }).catch(err => {
                    notifyError(err.message)
                })
                if (!_.isEmpty(declaration)) {
                    await TaxDeclarationService.getSettings(declaration.paySettingId).then((res) => {
                        settings = res.data
                    })
                }

                setState({ ...state, isLoading: false, financialYears: financialYears, financialYear: financialYear, declaration: declaration, settings: settings })

            }
        }
        fetchData();
    }, []);

    const handleEdit = (n) => {
        setOnEdit(!onEdit)
        // const qry = { r: (n.id ? crypto.encrypt(n.id) : '') };
        // props.history.push(`/m/Payroll/Declaration/updateTable?` + queryString.stringify(qry));
    }
    const handelRedirect = (data) => {
        setOnEdit(!onEdit)
        setState({ ...state, declaration: data });
    }
    const handleSave = () => {

    }

    return (
        <Fragment>
            {
                state.isLoading ? '' :
                    <Fragment>
                        <Card className="mb-3">
                            <CardBody>
                                <Row className='justify-content-left'>
                                    <Col md='4'>
                                        <Label >Financial Year</Label>
                                        <p><strong>{state.financialYear.name}</strong></p>
                                    </Col>
                                    <Col md='4'>
                                        <Label >Employee NO</Label>
                                        <p><strong>{state.declaration && state.declaration.no}</strong></p>
                                        <p><strong>{state.declaration && state.declaration.name}</strong></p>
                                    </Col>
                                    <Col md='4'>
                                        <Button className="mb-2 mr-2 btn-icon btn-primary" key='buttonCancel' color="success" type="button"
                                            name="cancel" onClick={() => history.goBack()} >
                                            <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i> Go to list
                                        </Button>
                                    </Col>
                                </Row>
                            </CardBody>
                        </Card>
                        {
                            onEdit ? <Fragment>
                                <Card>
                                    <CardBody>
                                        <DeclarationSummary declaration={state.declaration} />
                                        <Row className='justify-content-center'>
                                            <Button className="mb-2 mr-2 btn-icon btn-success1" key='buttonSubmit' color="success" type="button"
                                                name="submit" onClick={(e) => handleEdit()} >
                                                <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i>  Edit
                                            </Button>
                                            <Button className="mb-2 mr-2 btn-icon btn-primary" key='buttonCancel' color="success" type="button"
                                                name="cancel" onClick={() => history.goBack()} >
                                                <i className="pe-7s-paper-plane btn-icon-wrapper font-weight-bold text-black"> </i> Cancel
                                            </Button>
                                        </Row>
                                    </CardBody>
                                </Card>
                            </Fragment>
                                :
                                <Declaration {...props} declaration={state.declaration} fromMonth={state.financialYear.from}
                                    toMonth={state.financialYear.to} financialYearId={state.financialYear.id}
                                    handleSave={handleSave} selfDeclaration={false} handelRedirect={handelRedirect} />
                        }

                    </Fragment>
            }
        </Fragment>
    );
}

export default DeclarationUpdate;