import { PRIORITY } from 'Site_constants';
import Loading from 'components/Loader';
import { RWDropdownList } from 'components/dynamicform/Controls';
import DataTable from 'components/table/DataTable';
import UpdateTable from 'domain/HOC/withDataTable';
import React, { useEffect, useState } from 'react';
import { Button, Col, Row } from 'reactstrap';
import TicketLogService from 'services/HelpDesk/TicketLog';
import TicketStsService from 'services/HelpDesk/TicketSts';
import * as _ from 'lodash'
import EmployeeDetails from 'services/List/EmployeeDetails';

const title = 'Tickets';
const fields = [
    { name: 'no', label: 'Ticket No', type: 'input' },
    {
        name: 'raisedBy', label: 'Employee', type: 'custom',
        Cell: ({ value, row }) => {
            const currentRow = row.original
            currentRow['employeeName'] = value
            currentRow['department']=row.original.empDept
            return (
                <EmployeeDetails currentRow={currentRow} />
            )
        }
    },

    { name: 'department', label: 'Department', type: 'input' },
    { name: 'status', label: 'Status', type: 'select' },
    { name: 'helpTopic', label: 'Topic', type: 'input' },
    { name: 'subTopic', label: 'Sub Topic', type: 'input' },
    { name: 'subject', label: 'Subject', type: 'input' },
    { name: 'raisedOn', label: 'Raised At', type: 'datetime' },
    { name: 'priority', label: 'Priority', type: 'select' },
    { name: 'assignTo', label: 'Assigned To', type: 'input' },
    { name: 'lastModifiedAt', label: 'Last Updated', type: 'datetime' },
    { name: 'lastUpdatedBy', label: 'Last Updated By', type: 'input' },
];
export const STATUS = [{ value: 1, text: "Open" }, { value: 3, text: "Closed" }, { value: 4, text: 'In process' }]

const ddData = {
    priority: { entity: 'priority', data: [] },
};
ddData['priority'].data = PRIORITY;


function TicketsList(props) {
    const [state, setState] = useState({ isLoading: true, tickets: [] })
    useEffect(async () => {
        await TicketStsService.getList().then((res) => {
            let tts = _.orderBy(res.data, ['orderNo'], ['asc'])
            setState({ ...state, tickets: tts, isLoading: false })
        })
    }, [])
    const searchCard = () => {
        return (
            <Row>
                <Col md='2'>
                    <RWDropdownList {...{
                        name: 'ticketStsId', label: 'Status', valueField: 'id', textField: 'name',
                        value: props.searchData['ticketStsId'],
                        values: state.tickets
                    }} handlevaluechange={(e, v) => props.handleOnChange('ticketStsId', v)}
                    />
                </Col>
                <Col md='1' className='mt-3'>
                    <Button className="mr-2  btn-icon btn-icon-only btn-secondary btn-sm mb-4" onClick={(e) => props.handleSearch(0)}>
                        <i className="pe-7s-search btn-icon-wrapper"> </i></Button>
                </Col>
            </Row>
        )
    }
    return (
        state.isLoading ? <Loading /> : <DataTable {...props} searchCard={searchCard} />

    )
}
export default UpdateTable(TicketsList, { ...TicketLogService.paginate() }, fields, ddData, title, '')
