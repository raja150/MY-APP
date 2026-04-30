import React from 'react'
import {getCamelCaseObject} from 'utils/form' 

export default function LeaveTypeDetails(props) {
    var leaveTypes = JSON.parse(props.currentRow.leaveTypes)
    
    return (
        <div style={{ color: '#666' }}>
            <div>
                {leaveTypes != null && leaveTypes.map((leaveType, index) => {
                    leaveType = getCamelCaseObject(leaveType)
                    return (
                        <div style={{ fontSize: '12px' }}>
                            {leaveType.days > 0 && <span>{leaveType.name} : {leaveType.days} Day(s)</span>}<br />
                        </div>)
                })}
                {props.currentRow.lopDays > 0 &&
                    <div style={{ fontSize: '12px' }}>
                        <span>Lop Days : {props.currentRow.lopDays} Day(s)</span>
                    </div>
                }


            </div>
        </div>
    )

}