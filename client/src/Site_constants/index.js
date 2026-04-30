import styled from 'styled-components'
import SessionStorageService from 'services/SessionStorage';

export const TICKET_CATEGORY = [{ value: 1, text: 'Attendance' }, { value: 2, text: 'Shifts' }, { value: 3, text: 'CompOff' },
{ value: 4, text: 'Correction In Employee Details' }, { value: 5, text: 'Leaves' }]

export const GENDER = { 0: '', 1: 'Male', 2: 'Female' }


export const ATTENDANCE_STATUS = [{ value: 1, text: 'Present' }, { value: 2, text: 'Absent' }, { value: 3, text: 'Leave' },
{ value: 4, text: 'Week Off' }, { value: 5, text: 'WFH' }, { value: 6, text: 'Holiday' },
{ value: 7, text: 'Half Day Present' }, { value: 8, text: 'Half Day Leave' }, { value: 9, text: 'Half Day WFH' },
{ value: 10, text: 'Half Day Absent' }, { value: 11, text: 'Maternity Leave' }, { value: 12, text: 'Long Leave' },
{ value: 20, text: 'UnAuthorized Leave' }, { value: 21, text: 'Late' }]

export const EMPLOYEE_STATUS = [{ value: 1, text: 'Active' }, { value: 2, text: 'Resign' }]

export const att = [{ value: 'HalfDayLeave\nHalfDayAbsent', text: 'HDL & HDP' },
                    { value: 'HalfDayLeave\nHalfDayWFH', text: 'HDL & HDW' }]

export const RESIGNATION_TYPE = [{ value: 1, text: 'fire' }, { value: 2, text: 'Layoff' }, { value: 3, text: 'Termination by Mutual Agreement' },
{ value: 4, text: 'Voluntary Termination' }, { value: 5, text: 'Others' }]

export const EMPLOYEE_ADDRESS = [{ value: 1, text: 'Present Address' }, { value: 2, text: 'Permanent Address' }, { value: 3, text: 'Emergency Address' }]

export const GENDERS = [{ value: 1, text: 'Male' }, { value: 2, text: 'female' }, { value: 3, text: 'Others' }]

export const LEAVE_TYPE = [{ value: 1, text: 'Absent' }, { value: 2, text: 'Leave' }, { value: 4, text: 'Week Off' }, { value: 5, text: 'Unauthorized' }, { value: 7, text: 'Half-Day' }]

export const PRIORITY = [{ value: 1, text: 'Low' }, { value: 2, text: 'Normal' }, { value: 3, text: 'High' }, { value: 4, text: 'Emergency' }]
export const TICKET_STATUS = [{ value: 0, text: "Open" }, { value: 1, text: "My Tickets" }, { value: 2, text: "Overdue" }, { value: 3, text: "Closed" }, { value: 4, text: 'In process' }]

export const GENDER_M = [{ value: 1, text: 'Mr' }, { value: 2, text: 'Mrs' }, { value: 3, text: 'Mrs' }]
export const HUMAN_RELATION = { 1: 'Father', 2: 'Mother', 3: 'Spouse', 4: 'Child 1', 5: 'Child 2', 6: 'Child 3' }

export const MARITAL_STATUS = { 1: 'Married', 2: 'Single', 3: 'Separated' }
export const ApplyLeaveStatusValues = [{ value: 1, text: 'Applied' }, { value: 2, text: 'Approved' }, { value: 3, text: 'Rejected' }]

export const LEAVE_TYPE_DropDown = [{ value: 1, text: 'Present', isHalfDay: false, isLeaveDW: false }, { value: 2, text: 'Absent', isHalfDay: false, isLeaveDW: false }, { value: 3, text: 'Leave', isHalfDay: false, isLeaveDW: true }, { value: 4, text: 'Week Off', isHalfDay: false, isLeaveDW: false }, { value: 5, text: 'WFH', isHalfDay: false, isLeaveDW: false },
{ value: 6, text: 'Holiday', isHalfDay: false, isLeaveDW: false }, { value: 7, text: 'Half Day Present', isHalfDay: true, isLeaveDW: false }, { value: 8, text: 'Half Day Leave', isHalfDay: true, isLeaveDW: true }, { value: 9, text: 'Half Day WFH', isHalfDay: true, isLeaveDW: false }, { value: 10, text: 'Half Day Absent', isHalfDay: true, isLeaveDW: false }, { value: 20, text: 'Un Authorized', isHalfDay: false, isLeaveDW: false }]


export const Second_DropDown = [/*{value:1,text:[ { value: 2, text: 'Absent' }, { value: 3, text: 'Leave' }, { value: 4, text: 'Week Off' }, { value: 5, text: 'WFH' },{ value: 6, text: 'Holiday' }]},
                              {value:2,text:[ { value: 1, text: 'Present' }, { value: 3, text: 'Leave' }, { value: 4, text: 'Week Off' }, { value: 5, text: 'WFH' },{ value: 6, text: 'Holiday' }]},
                              {value:3,text:[ { value: 1, text: 'Present' }, { value: 2, text: 'Absent' }, { value: 4, text: 'Week Off' }, { value: 5, text: 'WFH' },{ value: 6, text: 'Holiday' }]},
                              {value:4,text:[ { value: 1, text: 'Present' }, { value: 2, text: 'Absent' }, { value: 3, text: 'Leave' }, { value: 5, text: 'WFH' },{ value: 6, text: 'Holiday' }]},
                              {value:5,text:[ { value: 1, text: 'Present' }, { value: 2, text: 'Absent' }, { value: 3, text: 'Leave' }, { value: 4, text: 'Week Off' },{ value: 6, text: 'Holiday' }]},
                              {value:6,text:[ { value: 1, text: 'Present' }, { value: 2, text: 'Absent' }, { value: 3, text: 'Leave' }, { value: 4, text: 'Week Off' },{ value: 5, text: 'WFH' }]},*/
  { value: 7, text: [{ value: 8, text: 'Half Day Leave', isLeaveDW: true }, { value: 9, text: 'Half Day WFH', isLeaveDW: false }, { value: 10, text: 'Half Day Absent', isLeaveDW: false }] },
  { value: 8, text: [{ value: 7, text: 'Half Day Present', isLeaveDW: false }, { value: 9, text: 'Half Day WFH', isLeaveDW: false }, { value: 10, text: 'Half Day Absent', isLeaveDW: false }] },
  { value: 9, text: [{ value: 7, text: 'Half Day Present', isLeaveDW: false }, { value: 8, text: 'Half Day Leave', isLeaveDW: true }, { value: 10, text: 'Half Day Absent', isLeaveDW: false }] },
  { value: 10, text: [{ value: 7, text: 'Half Day Present', isLeaveDW: false }, { value: 8, text: 'Half Day Leave', isLeaveDW: true }, { value: 9, text: 'Half Day WFH', isLeaveDW: false }] },
];

export const LEAVE_STATUS = [,
  { value: 1, text: 'Applied' },
  { value: 2, text: 'Approved' },
  { value: 3, text: 'Rejected' }]

export const CLIENT_VISIT_STATUS = [,
  { value: 1, text: 'Applied' },
  { value: 2, text: 'Approved' },
  { value: 3, text: 'Rejected' }]

export const ATTENDANCE_TYPE = { In: 0, Out: 1 }

export const CALENDAR_VIEW = { text: 'MONTH' }

export const COMPUTER_TYPES = [{ value: 1, text: 'Office Desktop' }, { value: 2, text: 'Office Laptop' }, { value: 3, text: 'BYOD' },
{ value: 4, text: 'BYOD RDP System' }, { value: 5, text: 'Office RDP System' }, { value: 6, text: 'Working from Office' }]

export const PERFORMANCE_TYPE = [{ value: 1, text: 'Monthly' }, { value: 2, text: 'Weekly' }]
export const PROVIDENT_FUND_INFO = [{ value: 1, text: 'All' }, { value: 2, text: 'PF Available' }, { value: 3, text: 'PF Not Available' }, { value: 4, text: 'PF Missing' }]
export const EMPLOYEE_CONTRIBUTION = [{ value: 0, text: ' ' }, { value: 1, text: '12% actual PF wages' }, { value: 2, text: 'Restrict to 15k of PF wages' }, { value: 3, text: 'As per organization' }]
export const ESI_INFO = [{ value: 1, text: 'All' }, { value: 2, text: 'ESI Available' }, { value: 3, text: 'ESI Not Available' }, { value: 4, text: 'ESI Missing' }]
export const PAYMENT_TYPE = [{ value: 1, text: 'Bank Transfer' }, { value: 2, text: 'Online' }]
export const REMOVE_SEARCH_DATA_WHEN_REFRESH = async () => {
  //checking if user is refreshed or not the existed filtered page
  if (window.performance.navigation.type == 1) {
    //REMOVE_SEARCH_DATA()
    return true;
  }
  return false
}
export const PERFORMANCE = [{ value: 0, text: 'All' }, { value: 1, text: 'Monthly' }, { value: 2, text: 'Weekly' }]
export const SEARCH_DATA = async () => {
  let dd = JSON.parse(SessionStorageService.getSearchData());
  return dd;
}
export const REMOVE_SEARCH_DATA = () => {
  SessionStorageService.removeSearchData();
}
export const TableComponent = styled.div`
.table th, .table td {
    padding: 0.35rem 0.55rem !important;
  }
  .table th, .table td {
    border-top: none;
    border-bottom: 1px solid #e9ecef;
  }
  .-pageJump select {
      width: 50px;
      border: 1px solid #ccc;
      border-radius: 4px;
      padding: 4px;
      margin: 0px 4px;
    }
    .resizer {
      right: 0;
      /* background: #000; */
      width: 1px;
      height: 100%;
      position: absolute;
      top: 0;
      z-index: 1;
      touch-action :none
    }
    .-cursor{
      cursor: pointer;
    }
`


