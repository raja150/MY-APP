import { RWDatePicker, AsyncTypeHead } from "components/dynamicform/Controls";
import DataTable from 'components/table/DataTable';
import UpdateTable from 'domain/HOC/withDataTable';
import queryString from 'query-string';
import React, { useEffect } from 'react';
import { VscOpenPreview } from 'react-icons/vsc';
import { Button, Card, CardBody, Col, Label, Row } from 'reactstrap';
import ResultService from 'services/OnlineTest/Result';
import * as crypto from 'utils/Crypto';
import { Progress } from 'reactstrap'
import { RESULT_EMPLOYEE, RESULT_PAPER } from '../Navigation'
const title = 'Result '

let fields = []

const ddData = ''

function List(props) {
    useEffect(() => {
        if (fields.length === 0) {
            fields.push(
                { name: 'employee', label: 'Employee', type: 'input', id: "Employee.Name" },
                { name: 'paperName', label: 'Paper', type: 'input', id: 'Paper.Name' },
                { name: 'testDate', label: 'Date', type: 'date', id: 'Paper.AddedAt' },
                {
                    name: 'percentage', label: 'Percentage', type: 'custom',
                    Cell: ({ value }) => {
                        return <div>
                            <Progress className="mt-1" color='success' value={value} > {value + '%'} </Progress>
                        </div>
                    }
                },
                {
                    name: 'view', label: 'View', type: 'custom', disableSorting: true,
                    Cell: ({ row }) => {
                        return <Button color='info' className='p-1 border-0 btn-transition $purple-600' outline
                            onClick={() => handleRedirect(row.original.id)}
                        >
                            <VscOpenPreview size='20px' />
                        </Button>
                    }
                }

            )
        }
    }, [])

    const handleRedirect = (resultId) => {
        props.history.push(`Summary` + '?' + queryString.stringify({ r: (crypto.encrypt(resultId)) }))
    }


    const searchCard = () => {
        return <Card className="main-card mb-2">
            <CardBody>
                <Row form className="w-100">
                    <Col md='3'>
                        <AsyncTypeHead {...{
                            name: 'employeeId', label: 'Employee',
                            valueField: 'id', textField: 'employeeName',
                            url: RESULT_EMPLOYEE,
                            placeholder: 'Search with employee name..'
                        }} handlevaluechange={(e, v) => props.handleOnChange(e, v)} />
                    </Col>
                    <Col md="3">
                        <AsyncTypeHead {...{
                            name: 'paperId', label: 'Paper',
                            valueField: 'id', textField: 'paperName',
                            url: RESULT_PAPER,
                            placeholder: 'Search with paper name...'
                        }} handlevaluechange={(e, v) => props.handleOnChange(e, v)} />
                    </Col>
                    <Col md="3">
                        <RWDatePicker {...{
                            name: 'testDate', label: 'Date',
                            showDate: true, format: "MM/DD/YYYY",
                            value: props.searchData && props.searchData.testDate,
                        }} handlevaluechange={props.handleOnChange} />
                    </Col>
                    <Col md="1" style={{ marginTop: '20px' }}>
                        <Button className="mr-2  btn-icon btn-icon-only btn-secondary btn-sm"
                            onClick={() => props.handleSearch()}>
                            <i className="pe-7s-search btn-icon-wrapper" />
                        </Button>
                    </Col>
                </Row>
            </CardBody>
        </Card>
    }
    return <DataTable {...props} searchCard={searchCard} hideAdd={true} />
}
export default UpdateTable(List, { ...ResultService.paginate() }, fields, ddData, title, true)