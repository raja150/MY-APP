// import React, { useState } from 'react';
// import { AsyncTypeahead } from 'react-bootstrap-typeahead';
// import SearchService from 'services/Search'

import { notifyError } from "components/alert/Toast";
import React, { useState } from "react";
import { AsyncTypeahead } from "react-bootstrap-typeahead";
import SearchService from 'services/Search';
import * as formUtil from 'utils/form'
export default function EmployeeTeamSearch(props) {
    const [isSearching, setIsSearching] = useState(false);
    const [state, setState] = useState({ Employees: [] })

    return (
        <AsyncTypeahead
            name={props.name}
            disabled={props.disabled}
            onChange={async (e) => {
                if (e.length > 0) {
                    const selEmployee = e[0];
                    props.setSelEmp(selEmployee)
                    props.handleValueChange && await props.handleValueChange('employeeId', selEmployee.id, selEmployee)
                    props.handleValueChange && await props.handleValueChange('no', selEmployee.no , selEmployee)

                }
                else {
                    props.setSelEmp({})
                    props.handleValueChange && await props.handleValueChange('employeeId', '', '')
                    await props.handleValueChange('no', '','')
                }
            }}

            onSearch={async (query) => {
                setIsSearching(true);
                await SearchService.SearchWithEmpTeam(query).then((res) => {
                    setState({ ...state, Employees: res.data })
                }).catch((err) => {
                    formUtil.displayAPIError(err)
                    //notifyError("Login user doesnot belongs ro any team")
                })
                setIsSearching(false);
            }}

            renderMenuItemChildren={(option) => {
                return [
                    <div key={option.no}>
                        <div><strong>Name:{option.name}</strong></div>
                        <div>Employee No:{option.no}</div>
                        <div>Designation : {option.designation}</div>
                        <div>Department : {option.department}</div>
                    </div>
                ]
            }}
            isLoading={isSearching}
            options={state.Employees ? state.Employees : []}
            labelKey={option => `${option.name}`}
            selected={props.selEmp && props.selEmp.id ? [props.selEmp] : []}
            id='id'
            minLength={4} 
        />
    )
}