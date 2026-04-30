import React from 'react';
import DataTable from '../../../components/table/DataTable';
import UpdateTable from '../../HOC/withDataTable';
import { Button, Card, CardBody, Col, Label, Row } from 'reactstrap';
import { DateTimePicker } from 'react-widgets';
import moment from 'moment';
import ApplyLeavesService from 'services/Leave/ApplyLeave'

const title = ''
const onEdit = true;
const fields = [
    { name: 'leaveDate', label: 'Leave Date', type: 'date' },
    { name: 'leaveName', label: 'Leave Type', type: 'input' },
    {
        name: 'leaveCount', label: 'Leave Day(s)', type: 'custom',
        Cell: ({ row }) => {
            return (
                <span>{(row.original.leaveCount).toFixed(1)}</span>
            )
        }
    }
]
const type = ''

function LeaveRequestDetailsList(props) {
    const searchCard = () => {
        return (
            <Card className='mb-1'>
            </Card>
        )
    }
    return (
        <DataTable {...props} searchCard={searchCard} hideAdd={true} />
    )

}
export default UpdateTable(LeaveRequestDetailsList, ApplyLeavesService.RequestDetailsPaginate(), fields, {}, title, onEdit, "Hide PopUp", type)