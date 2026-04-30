import React from 'react'
export default function EmployeeDetails(props) {
    return (
        <div>
            <div>
                <span style={{ color: '#458bd6' }}>{props.currentRow.employeeName}</span>
                <div style={{ fontSize: '10px' }}>
                    <span>{props.currentRow.employeeNo}</span><br />
                    <span>{props.currentRow.designation}</span><br />
                    <span>{props.currentRow.department}</span><br />
                </div>
            </div>
        </div>
    )

}