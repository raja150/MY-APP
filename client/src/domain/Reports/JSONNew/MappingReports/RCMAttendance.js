import { ATTENDANCE_STATUS, EMPLOYEE_STATUS } from "Site_constants";

export const RCMAttendance = {
    "id": "834ed77e-73ab-4fe4-9c74-f7181fa336e6",
    "name": "RCMAttendance",
    "label": "RCM Attendance",
    "url": 'LMSReport/RCMAttendance',
    "type": 1,
    "orderBy": 0,
    "filters": [
        {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "lobId",
            "label": "Line of business",
            "field": "Dropdown",
            "url": "LmsReport/EmployeeLOB",
            "type": "int",
            "valueField": "id",
            "textField": "name",
            "data": [],
            "orderBy": 1
        },
        {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "functionalAreaId",
            "label": "Functional Area",
            "field": "Dropdown",
            "url": "LmsReport/FunctionalArea",
            "type": "int",
            "valueField": "id",
            "textField": "name",
            "data": [],
            "orderBy": 2
        },
        {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "designationId",
            "label": "Designation",
            "field": "Dropdown",
            "url": "LmsReport/Designations",
            "type": "int",
            "valueField": "id",
            "textField": "name",
            "data": [],
            "orderBy": 3
        },
        {
            "id": "425cb45d-5a6d-4cb3-9148-14f820f3e864",
            "name": "fromDate",
            "label": "Attendance From Date",
            "field": "DatePicker",
            "url": "",
            "type": "Date",
            "data": [],
            "orderBy": 4
        },
        {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "toDate",
            "label": "Attendance To Date",
            "field": "DatePicker",
            "url": "",
            "type": "Date",
            "data": [],
            "orderBy": 5
        },
        {
            "id": "fa94d2d1-0c4f-4bdc-98c6-3ab0aa0c0800",
            "name": "employeeStatus",
            "label": "Employee Status",
            "field": "Dropdown",
            "type": "int",
            "data": EMPLOYEE_STATUS,
            "valueField": "value",
            "textField": "text",
            "orderBy": 6
        },
    ],
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
            "name": "department",
            "label": "Department",
            "type": "string",
            "data": [],
            "orderBy": 1,
            "status": true
        },
        {
            "name": "lob",
            "label": "Line of Business",
            "type": "string",
            "data": [],
            "orderBy": 2,
            "status": true
        },
        {
            "name": "fa",
            "label": "Functional Area",
            "type": "string",
            "data": [],
            "orderBy": 3,
            "status": true
        },
        {
            "name": "designation",
            "label": "Designation",
            "type": "string",
            "data": [],
            "orderBy": 4,
            "status": true
        },
        {
            "name": "date",
            "label": "Date",
            "type": "Date",
            "data": [],
            "orderBy": 5,
            "status": true,
        },
        {
            "name": "punchIn",
            "label": "Punch In",
            "type": "DateinHoursMins",
            "data": [],
            "orderBy": 6,
            "status": true
        },
        {
            "name": "punchOut",
            "label": "Punch Out",
            "type": "DateinHoursMins",
            "data": [],
            "orderBy": 7,
            "status": true
        },
        {
            "name": "workTime",
            "label": "Work Time",
            "type": "TimeInHours",
            "data": [],
            "orderBy": 8,
            "status": true
        },
        {
            "name": 'attendanceStatus',
            "label": "Attendance Status",
            "type": "string",
            "data": ATTENDANCE_STATUS,
            "orderBy": 9,
            "status": true
        },
    ]
}