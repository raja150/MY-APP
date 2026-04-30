export const EmployeeShift = {
    "id": "834ed77e-73ab-4fe4-9c74-f7181fa336e6",
    "name": "ShiftAndWeekOffReport",
    "label": "ShiftAndWeekOffReport",
    "url": 'LMSReport/Shift',
    "type": 1,
    "orderBy": 0,
    "filters": [
        {
            "id": "fa94d2d1-0c4f-4bdc-98c6-3ab0aa0c0800",
            "name": "departmentId",
            "label": "Department",
            "field": "Dropdown",
            "url": "Organization/Department/GetList",
            "type": "int",
            "valueField": "id",
            "textField": "name",
            "data": [],
            "orderBy": 1
        },
        {
            "id": "fa94d2d1-0c4f-4bdc-98c6-3ab0aa0c0800",
            "name": "designationId",
            "label": "Designation",
            "field": "Dropdown",
            "url": "Organization/Designation/GetList",
            "type": "int",
            "valueField": "id",
            "textField": "name",
            "data": [],
            "orderBy": 2
        },
        {
            "id": "fa94d2d1-0c4f-4bdc-98c6-3ab0aa0c0800",
            "name": "teamId",
            "label": "Team",
            "field": "Dropdown",
            "url": "Organization/Team/GetList",
            "type": "int",
            "valueField": "id",
            "textField": "name",
            "data": [],
            "orderBy": 3
        },
        {
            "id": "fa94d2d1-0c4f-4bdc-98c6-3ab0aa0c0800",
            "name": "employeeId",
            "label": "Employee",
            "field": "AsyncDropdown",
            "url": "Search/SearchByEmployee",
            "type": "int",
            "valueField": "id",
            "textField": "name",
            "data": [],
            "orderBy": 4
        },
        {
            "id": "fa94d2d1-0c4f-4bdc-98c6-3ab0aa0c0800",
            "name": "shiftId",
            "label": "Shift",
            "field": "Dropdown",
            "url": 'Leave/Shift/GetList',
            "type": "int",
            "data": [],
            "valueField": "id",
            "textField": "name",
            "orderBy": 5
        },
    ],
    "columns": [
        {
            "name": "no",
            "label": "Employee No",
            "type": "string",
            "data": [],
            "orderBy": 1,
            "status": true
        },
        {
            "name": "name",
            "label": "Employee Name",
            "type": "string",
            "data": [],
            "orderBy": 2,
            "status": true
        },
        {
            "name": "department",
            "label": "Department",
            "type": "string",
            "data": [],
            "orderBy": 4,
            "status": true
        },
        {
            "name": "designation",
            "label": "Designation",
            "type": "string",
            "data": [],
            "orderBy": 5,
            "status": true
        },
        {
            "name": "doj",
            "label": "DOJ",
            "type": "Date",
            "data": [],
            "orderBy": 6,
            "status": true
        },
        {
            "name": "shift",
            "label": "Shift Name",
            "type": "string",
            "data": [],
            "orderBy": 7,
            "status": true
        }
    ]
}