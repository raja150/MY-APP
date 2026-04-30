import React from 'react'
import DataTable from '../../../components/table/DataTable';
import UpdateTable from '../../HOC/withDataTable';
import CompensatoryWorkingDayService from 'services/Leave/CompensatoryWorkingDayService';
import { StatusValues } from 'domain/Approvals/ApplyCompensatory/ConstValue';
import DateDetails from 'services/List/DateDetails';
import StatusDetails from 'services/List/StatusDetails';

const fields = [
    {
        name: 'fromDate', label: 'From Date', type: 'custom',
        Cell: ({ value, row }) => {
            return <DateDetails value={value} row={row} />
        }
    },
    {
        name: 'status', label: 'Status', type: 'custom',
        Cell: ({ value, row }) => {
            return <StatusDetails value={value} row={row}/>
        }
    },
]
const title = 'Compensatory Working Day ';
const ddData = { status: { entity: 'status', data: [] } };
ddData['status'].data = StatusValues;
function WorkingDayList(props) {
    const searchCard = () => {
        return (
            <></>
        )
    }
    return (

        <DataTable {...props} searchCard={searchCard} />
    )

}
export default UpdateTable(WorkingDayList, CompensatoryWorkingDayService.SelfServicePaginate(), fields, ddData, title, '', '')