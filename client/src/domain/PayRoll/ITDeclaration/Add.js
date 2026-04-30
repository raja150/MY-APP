import { RWDropdownList } from 'components/dynamicform/Controls';
import EmployeeSearch from 'domain/EmployeeSearch';
import queryString from 'query-string';
import React, { useEffect, useState } from 'react';
import { Card, CardBody, Col, Label, Row } from 'reactstrap';
import TaxDeclarationService from 'services/PayRollServices/TaxDeclarationService';
import * as crypto from '../../../utils/Crypto';
import Declaration from './index';
import GoBack from 'components/Button/GoBack';
import { DECLARATION } from '../navigation';

function AddDeclaration(props) {

    const [state, setState] = useState({ isLoading: true, financialYear: [], selectedFYId: '', fromMonth: '', toMonth: '' });
    const [selEmp, setSelEmp] = useState('')
    const [isSearching, setIsSearching] = useState(false);

    useEffect(() => {
        const fetchData = async () => {
            let financialYear = [];
            await TaxDeclarationService.getFinancialYear().then((result) => {
                financialYear = result.data;
            })

            setState({ ...state, isLoading: false, financialYear: financialYear });
        }
        fetchData();
    }, [])

    const handleSave = (data) => {

    }

    const handelRedirect = (data) => {
        const qry = { r: (data.id ? crypto.encrypt(data.id) : '') };
        props.history.push('/m/Payroll/Declaration/Update?' + queryString.stringify(qry));
        //props.history.push(`/m/Payroll/Declaration/Update?`);
    }
    const handleValueChange = async (name, value, { selected }) => {
        if (name === 'selectedFYId') {
            setState({ ...state, fromMonth: selected.from, toMonth: selected.to, [name]: value })
        } else {
            setState({ ...state, [name]: value });
        }

    };
    return (
        <>
            <GoBack title={'Declaration'} link={DECLARATION} />
            <Card className="mb-3">
                <CardBody>
                    <Row>
                        <Col>
                            <RWDropdownList {...{
                                name: 'selectedFYId', label: 'Financial Year', valueField: 'id', textField: 'name',
                                value: state.selectedFYId, type: 'string', values: state.financialYear,
                            }} handlevaluechange={handleValueChange} />
                        </Col>
                        <Col>
                            <Label>Employee Name</Label>
                            <EmployeeSearch name={'employeeID'} disabled={false} selEmp={selEmp} setSelEmp={setSelEmp} handleValueChange={handleValueChange} />
                            {/* <AsyncTypeahead
                                name='employeeID'
                                id='employeeID'
                                onChange={async (e) => {
                                    if (e.length > 0) {
                                        const selEmployee = e[0];
                                        setSelEmp(selEmployee) 
                                    }
                                    else {
                                        setSelEmp('')
                                    }
                                }}
                                onSearch={async (query) => {
                                    setIsSearching(true);
                                    await SearchService.SearchWithEmpName(query).then((res)=>{
                                        setState({ ...state, Employees: res.data })
                                    })
                                    setIsSearching(false);
                                }}
                                isLoading={isSearching}
                                options={state.Employees ? state.Employees : []}
                                labelKey={option => `${option.name}`}
                                selected={selEmp && selEmp.id ? [selEmp] : []}
                                minLength={4}
                            /> */}
                        </Col>
                    </Row>
                </CardBody>
            </Card>
            {selEmp && state.selectedFYId ? <Declaration {...props} fromMonth={state.fromMonth} toMonth={state.toMonth}
                handleSave={handleSave} selectedFYId={state.selectedFYId} employeeID={selEmp.id}
                selfDeclaration={false} handelRedirect={handelRedirect} /> : ''}

        </>
    )
}
export default AddDeclaration;