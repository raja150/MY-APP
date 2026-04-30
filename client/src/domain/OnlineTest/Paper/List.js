import { AsyncTypeHead, RWDropdownList } from 'components/dynamicform/Controls';
import DataTable from 'components/table/DataTable';
import UpdateTable from 'domain/HOC/withDataTable';
import React from 'react';
import { Button, Card, CardBody, Col, Row } from 'reactstrap';
import PaperService from 'services/OnlineTest/Paper';
import { IS_IN_LIVE, LIVE_VALUE, SEARCH_PAPER, STATUS, STATUS_VALUE } from './Constants';

const title = 'Paper'


const fields = [
    { name: 'name', label: ' Name', type: 'input' },
    { name: 'duration', label: 'Duration (mins)', type: 'input' },
    { name: 'startAt', label: 'Start-At', type: 'date' },
    { name: 'endAt', label: 'End-At', type: 'date' },
    {
        name: 'status', label: 'Status', type: 'custom',
        Cell: ({ value }) => {
            return <div>{STATUS_VALUE(value)}</div>
        }
    },
    {
        name: 'moveToLive', label: 'IsIn-Live', type: 'custom',
        Cell: ({ value }) => {
            return <div>{LIVE_VALUE(value)}</div>
        }
    }
]

const ddData = ''

function PaperList(props) {

    const searchCard = () => {
        return <Card className="main-card mb-2">
            <CardBody>
                <Row form className="w-100">
                    <Col md="3">
                        <AsyncTypeHead {...{
                            name: 'paperId', label: 'Paper ',
                            valueField: 'paperId', textField: 'paper',
                            url: SEARCH_PAPER,
                        }} handlevaluechange={(e, v) => props.handleOnChange(e, v)} />
                    </Col>
                    <Col md="3">
                        <RWDropdownList {...{
                            name: 'status', label: 'Status',
                            valueField: 'value', textField: 'text',
                            values: STATUS,
                            value: props.searchData && props.searchData.status,
                        }} handlevaluechange={props.handleOnChange} />
                    </Col>
                    <Col md="3">
                        <RWDropdownList {...{
                            name: 'isinLive', label: 'isIn-Live',
                            valueField: 'value', textField: 'text',
                            values: IS_IN_LIVE,
                            value: props.searchData && props.searchData.isinLive,
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
    return <DataTable {...props} searchCard={searchCard} />
}
export default UpdateTable(PaperList, { ...PaperService.paginate() }, fields, ddData, title, '')