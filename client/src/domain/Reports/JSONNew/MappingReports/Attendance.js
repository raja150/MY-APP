import { ATTENDANCE_STATUS } from "Site_constants";

export const Attendance = {
  "id": "834ed77e-73ab-4fe4-9c74-f7181fa336e6",
  "name": "attendance",
  "label": "Attendance",
  "url": 'LMSReport/Attendance',
  "type": 1,
  "orderBy": 0,
  "filters": [
    {
      "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
      "name": "departmentId",
      "label": "Department",
      "field": "Dropdown",
      "url": "Organization/Department/GetList",
      "type": "int",
      "valueField": "id",
      "textField": "name",
      "data": [],
      "orderBy": 0
    },
    {
      "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
      "name": "designationId",
      "label": "Designation",
      "field": "Dropdown",
      "url": "Organization/Designation/GetList",
      "type": "int",
      "valueField": "id",
      "textField": "name",
      "data": [],
      "orderBy": 1
    },
    {
      "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
      "name": "teamId",
      "label": "Team",
      "field": "Dropdown",
      "url": "Organization/Team/GetList",
      "type": "int",
      "valueField": "id",
      "textField": "name",
      "data": [],
      "orderBy": 2
    },
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
    }
  ],
  "columns": [
    {
      "name": "name",
      "label": "Name",
      "type": "string",
      "data": [],
      "orderBy": 0,
      "status": true,
    },
    {
      "name": "date",
      "label": "Date",
      "type": "Date",
      "data": [],
      "orderBy": 1,
      "status": true
    },
    {
      "name": "punchIn",
      "label": "Punch In",
      "type": "DateinHoursMins",
      "data": [],
      "orderBy": 2,
      "status": true
    },
    {
      "name": "punchOut",
      "label": "Punch Out",
      "type": "DateinHoursMins",
      "data": [],
      "orderBy": 4,
      "status": true
    },
    {
      "name": "workTime",
      "label": "Work Time",
      "type": "TimeInHours",
      "data": [],
      "orderBy": 5,
      "status": true
    },
    {
      "name": "breakTime",
      "label": "Break Time",
      "type": "TimeInHours",
      "data": [],
      "orderBy": 6,
      "status": true
    },
    {
      "name": "attendanceStatus",
      "label": "Attendance Status",
      "type": "string",
      "data": ATTENDANCE_STATUS,
      "orderBy": 7,
      "status": true
    },
  ]
}
