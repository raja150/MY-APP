export const MyTeamLeaveBalance= {
    "id": "834ed77e-73ab-4fe4-9c74-f7181fa336e6",
    "name": "LeaveBalances",
    "label": "Leave Balances",
    "url": 'LMSReport/MyTeamLeaveBalances',
    "type": 1,
    "orderBy": 0,
    "filters": [
     
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
            "name": "employeeId",
            "label": "Employee",
            "field": "AsyncDropdown",
            "url": "Search/SearchByEmployee",
            "type": "int",
            "valueField":"id",
            "textField":"name",
            "data": [],
            "orderBy": 2
          },
        {
            "id": "fa94d2d1-0c4f-4bdc-98c6-3ab0aa0c0800",
            "name": "leaveTypeId",
            "label": "Leave Type",
            "field": "Dropdown",
            "url": 'Leave/LeaveType/GetList',
            "type": "int",
            "data": [],
            "valueField": "id",
            "textField": "name",
            "orderBy": 3
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
            "name": "designation",
            "label": "Designation",
            "type": "string",
            "data": [],
            "orderBy": 3,
            "status": true
        },
        {
            "name": "leaveType",
            "label": "Leave Type",
            "type": "string",
            "data": [],
            "orderBy": 4,
            "status": true
        },
        {
            "name": "balance",
            "label": "Balance",
            "type": "string",
            "data": [],
            "orderBy": 5,
            "status": true
        },
    ]
}