import { PAYMENT_TYPE } from "Site_constants";

export const PaymentInfo= {
    "id": "834ed77e-73ab-4fe4-9c74-f7181fa336e6",
    "name": "paymentInfo",
    "label": "Payment Info",
    "url": 'PayRollReport/PaymentInfo',
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
            "valueField":"id",
            "textField":"name",
            "data": [],
            "orderBy": 3
          },
    ],
    "columns": [
        {
            "name": "employeeCode",
            "label": "Employee Code",
            "type": "string",
            "data": [],
            "orderBy": 1,
            "status": true
        },
        {
            "name": "employeeName",
            "label": "Employee Name ",
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
            "name": "dateOfJoining",
            "label": "Date Of Joining",
            "type": "Date",
            "data": [],
            "orderBy": 4,
            "status": true
        },
        {
            "name": "department",
            "label": "Department",
            "type": "string",
            "data": [],
            "orderBy": 5,
            "status": true
        },
        {
            "name": "payMode",
            "label": "Pay Mode",
            "type": "string",
            "data": PAYMENT_TYPE,
            "orderBy": 6,
            "status": true
        },
        {
            "name": "bank",
            "label": "Employer Bank",
            "type": "string",
            "data": [],
            "orderBy": 7,
            "status": true
        },
        {
            "name": "bankName",
            "label": "Bank Name",
            "type": "string",
            "data": [],
            "orderBy": 8,
            "status": true
        },
        {
            "name": "ifscCode",
            "label": "IFSC Code",
            "type": "string",
            "data": [],
            "orderBy": 9,
            "status": true
        },
        {
            "name": "accountNo",
            "label": "Account No",
            "type": "string",
            "data": [],
            "orderBy": 10,
            "status": true
        }
    ]
}
