import { ATTENDANCE_STATUS } from "Site_constants";
export const ReportingToAttendance = {
    "id": "834ed77e-73ab-4fe4-9c74-f7181fa336e6",
    "name": "MyTeamAttendance",
    "label": "MyTeamAttendance",
    "url": 'LMSReport/ReportingToAttendance',
    "summaryTableUrl": 'LM_Attendance/Attendance/TeamSummary',
    "type": 1,
    "orderBy": 0,
    "filters": [
        {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "employeeId",
            "label": "Employee",
            "field": "AsyncDropdown",
            "url": "Search/SearchByEmployee",
            "type": "int",
            "valueField": "id",
            "textField": "name",
            "data": [],
            "orderBy": 1
        },
        {
            "id": "425cb45d-5a6d-4cb3-9148-14f820f3e864",
            "name": "fromDate",
            "label": "From Date",
            "field": "DatePicker",
            "url": "",
            "type": "Date",
            "data": [],
            "orderBy": 2,
            "default": ''
        },
        {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "toDate",
            "label": "To Date",
            "field": "DatePicker",
            "url": "",
            "type": "Date",
            "data": [],
            "orderBy": 3,
            "default": ''
        },
        {
            "id": "fa94d2d1-0c4f-4bdc-98c6-3ab0aa0c0800",
            "name": "attendanceStatus",
            "label": "Attendance Status",
            "field": "Dropdown",
            "type": "int",
            "data": ATTENDANCE_STATUS,
            "valueField": "value",
            "textField": "text",
            "orderBy": 4
        },
    ],
    "popUpfilters": ["date", "loginType", "employeeId"],
    "columns": [
        {
            "name": "name",
            "label": "Name",
            "type": "string",
            "data": [],
            "orderBy": 0,
            "status": true
        },
        {
            "name": "date",
            "label": "Date",
            "type": "Date",
            "data": [],
            "orderBy": 1,
            "status": true,
            "popUpUrl": 'LM_Attendance/Attendance/Movement',
            //to disable popup link (based on punchIn data)
            "validation": 'punchIn',
            "hasPopUp": [
                {
                    "name": "movementTime",
                    "label": "Time",
                    "type": "DateinHoursMins",
                    "data": [],
                    "orderBy": 1,
                    "status": true
                },
                {
                    "name": "movementType",
                    "label": "Movement Type",
                    "type": "string",
                    "data": [{ value: 0, text: "In" }, { value: 1, text: "Out" }],
                    "orderBy": 2,
                    "status": true,
                }
            ],
        },
        {
            "name": "punchIn",
            "label": "Punch In",
            "type": "DateinHoursMins",
            "data": [],
            "orderBy": 2,
            "status": true,
        },
        {
            "name": "punchOut",
            "label": "Punch Out",
            "type": "DateinHoursMins",
            "data": [],
            "orderBy": 3,
            "status": true
        },
        {
            "name": "workTime",
            "label": "Work Time",
            "type": "TimeInHours",
            "data": [],
            "orderBy": 4,
            "status": true,
            "popUpUrl": 'LM_Attendance/Attendance/Movement',
        },
        {
            "name": "breakTime",
            "label": "Break Time",
            "type": "TimeInHours",
            "data": [],
            "orderBy": 5,
            "status": true
        },
        {
            "name": 'attendanceStatus',
            "label": "Attendance Status",
            "type": "string",
            "data": ATTENDANCE_STATUS,
            "orderBy": 6,
            "status": true
        },
    ],
    "summaryTable": [
        {
            "name": "status",
            "label": "Attendance Status",
            "type": "string",
            "data": ATTENDANCE_STATUS,
            "orderBy": 1,
            "status": true
        },
        {
            "name": "noOfDays",
            "label": "Days",
            "type": "int",
            "data": [],
            "orderBy": 2,
            "status": true,
        }
    ],
}