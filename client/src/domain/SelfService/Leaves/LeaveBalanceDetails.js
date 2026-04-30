import React from 'react';
import { Card } from 'reactstrap';
import ReportService from 'services/SelfService/ReportService';
import DataTable from '../../../components/table/DataTable';
import UpdateTable from '../../HOC/withDataTable';

const title = ''
const onEdit = true;
const fields = [
    { name: 'name', label: 'Leave Type', type: 'input' },
    {
        name: 'leaves', label: 'Count', type: 'custom',
        Cell: ({ row }) => {
            return (
                <span>{(row.original.leaves).toFixed(1)}</span>
            )
        }
    },
    { name: 'effectiveFrom', label: 'Valid From', type: 'date' },
    { name: 'effectiveTo', label: 'Valid To', type: 'date' },
]
const type = ''

function LeaveBalanceDetailsList(props) {
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
export default UpdateTable(LeaveBalanceDetailsList, ReportService.getLeaveBalanceDetailsPaginate(), fields, {}, title, onEdit, "Hide PopUp", type)