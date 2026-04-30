import React from 'react'
import DataTable from '../../../components/table/DataTable';
import UpdateTable from '../../HOC/withDataTable';
import RoleService from 'services/Roles';

const title = 'Roles'

const fields = [
    {
        name: 'name', label: 'Name', type: 'string'
    }
]
function RoleList(props) {
    const searchCard = () => {
        return (
            <div></div>
        )
    }
    return (
        <DataTable {...props} searchCard={searchCard} />
    )

}
export default UpdateTable(RoleList, {...RoleService.paginate()}, fields, '', title, '')
