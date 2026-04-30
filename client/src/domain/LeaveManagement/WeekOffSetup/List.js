import React from 'react';
import WeekOffSetupService from '../../../services/Leave/WeekOffSetup'
import DataTable from '../../../components/table/DataTable';
import UpdateTable from '../../HOC/withDataTable';
import { LEAVE_TYPE_STATUS } from '../navigation';

const title = 'Week Off Setup'
const fields = [
    { name: 'name', label: 'Name', type: 'input' },
    { name: 'status', label: 'Status', type: 'select' },
]

const ddData = { status: { entity: 'status', data: [] } };
ddData['status'].data = LEAVE_TYPE_STATUS;

function WeekOffSetup(props) {
    const searchCard = () => {
        return (
            <p></p>
        )
    }
    return (
        <DataTable {...props} searchCard={searchCard} />
    )

}
export default UpdateTable(WeekOffSetup, WeekOffSetupService.paginate(), fields,ddData, title, '', '')