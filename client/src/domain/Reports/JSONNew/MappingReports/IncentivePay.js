export const IncentivePay= {
    "id": "834ed77e-73ab-4fe4-9c74-f7181fa336e6",
    "name": "IncentivePay",
    "label": "IncentivePay",
    "url": 'PayRollReport/IncentivePay',
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
          {
            "id": "2fd2b67d-1291-40e3-aff2-494da6011748",
            "name": "payMonthAndYear",
            "label": "Pay Month And Year",
            "field": "MonthYearPicker",
            //"url": "PayRoll/PayMonth/GetAllMonths",
            "type": "Date",
            "editFormat" : "MM/YY",
            "format":"MM/YY",
            "data": [],
            "orderBy": 4,
            "showDate":1,
            "showTime":0,
            "views": ["year", "decade", "century"],
            
          },
    ],
    "columns": [
        {
            "name": "empCode",
            "label": "Employee No",
            "type": "string",
            "data": [],
            "orderBy": 1,
            "status": true
        },
        {
            "name": "name",
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
            "name": "month",
            "label": "Month",
            "type": "string",
            "data": [],
            "orderBy": 5,
            "status": true
        },
        {
            "name": "faxFilesAndArrears",
            "label": "Fax Files and Arrears",
            "type": "string",
            "data": [],
            "orderBy": 6,
            "status": true
        },
        {
            "name": "sundayInc",
            "label": "Sunday Incentives",
            "type": "string",
            "data": [],
            "orderBy": 7,
            "status": true
        },
        {
            "name": "productionInc",
            "label": "Production Incentives",
            "type": "string",
            "data": [],
            "orderBy": 8,
            "status": true
        },
        {
            "name": "spotInc",
            "label": "Spot Incentives",
            "type": "string",
            "data": [],
            "orderBy": 9,
            "status": true
        },
        {
            "name": "punctualityInc",
            "label": "Punctuality Incentive",
            "type": "string",
            "data": [],
            "orderBy": 10,
            "status": true
        },
        {
            "name": "centumClub",
            "label": "Centum Club/Double Centum Club",
            "type": "string",
            "data": [],
            "orderBy": 11,
            "status": true
        },{
            "name": "firstMinuteInc",
            "label": "First Minute Incentive",
            "type": "string",
            "data": [],
            "orderBy": 12,
            "status": true
        },
        {
            "name": "otherInc",
            "label": "Other Incentives",
            "type": "string",
            "data": [],
            "orderBy": 13,
            "status": true
        },
        {
            "name": "nightShift",
            "label": "Night Shift",
            "type": "string",
            "data": [],
            "orderBy": 14,
            "status": true
        },
        {
            "name": "weeklyStarInc",
            "label": "Weekly Star Incentives",
            "type": "string",
            "data": [],
            "orderBy": 15,
            "status": true
        },{
            "name": "tTeamInc",
            "label": "Transition Team Incentives",
            "type": "string",
            "data": [],
            "orderBy": 16,
            "status": true
        },
        {
            "name": "doublePay",
            "label": "Double Pay",
            "type": "string",
            "data": [],
            "orderBy": 17,
            "status": true
        },
        {
            "name": "internalQualityFeedbackDed",
            "label": "Internal Quality Feedback",
            "type": "string",
            "data": [],
            "orderBy": 18,
            "status": true
        },
        {
            "name": "externalQualityFeedbackDed",
            "label": "External Quality Feedback",
            "type": "string",
            "data": [],
            "orderBy": 19,
            "status": true
        },{
            "name": "lateComingDed",
            "label": "Late Coming Deductions",
            "type": "string",
            "data": [],
            "orderBy": 20,
            "status": true
        },
        {
            "name": "unauthorizedLeaveDed",
            "label": "Unauthorized Leave Deduction",
            "type": "string",
            "data": [],
            "orderBy": 21,
            "status": true
        },
        {
            "name": "otherDed",
            "label": "Other Deductions",
            "type": "string",
            "data": [],
            "orderBy": 22,
            "status": true
        },
    ]
}
