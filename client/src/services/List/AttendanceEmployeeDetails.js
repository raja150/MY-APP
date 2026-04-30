import React from 'react'
import * as dateUtil from 'utils/date';
export default function EmployeeDetails(props) {
    return (
        <div>
            <div>
                <span style={{ color: '#458bd6' }}>{props.currentRow.name}</span>
                <div style={{ fontSize: '10px' }}>
                    <span>{props.currentRow.employeeNo}</span><br />
                    <span>{props.currentRow.designation}</span><br />
                    <span>{dateUtil.getShortDate(props.currentRow.dateOfJoining)}</span>
                </div>
            </div>
        </div>
    )

}