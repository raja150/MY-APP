import DataImport from 'domain/Setup/DataImport'
import React from 'react'
const IMPORT_TYPES = [
    { value: 1, text: 'Leave Balance', postURL: 'LeaveBalance', type: 'Leave_Balance', fileName: '' }, 
    { value: 2, text: 'Attendance Logs Import', postURL: 'AttendanceLogsImport', type: 'AttendanceLogsImport', fileName: '' },
    { value: 3, text: 'Update Attendance', postURL: 'AttendanceUpdate', type: 'AttendanceUpdate', fileName: '' },
    { value: 4, text: 'Employee', postURL: 'Employee', type: 'Employee', fileName: '' },
    { value: 5, text: 'Add Attendance', postURL: 'AddAttendance', type: 'AddAttendance', fileName: '' },

]
function LeaveDataImport(props){
    
    return (
        <DataImport IMPORT_TYPES={IMPORT_TYPES}/>
    )
}
export default LeaveDataImport;   