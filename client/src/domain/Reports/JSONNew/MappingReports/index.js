import { AllActiveEmployees } from "./AllActiveEmployees"
import { Arrears } from "./Arrears"
import { Attendance } from "./Attendance"
import { EmployeeAddress } from "./EmployeeAddress"
import { EmployeeContactDetails } from "./EmployeeContactDetails"
import { EmployeeESI } from "./EmployeeESI"
import { EmployeeEPF } from "./EmployeeEPF"
import { IncentivePay } from "./IncentivePay"
import { IncomeTax } from "./IncomeTax"
import { LeaveBalanced } from "./LeaveBalance"
import { MyTeamAttendance } from "./MyTeamAttendance"
import { MyTeamLeaveBalance } from "./MyTeamLeaveBalance"
import { ProfessionalTax } from "./ProfessionalTax"
import { ResignedEmployees } from "./ResignedEmployee"
import { EmployeeWeekOff } from './EmployeeWeekOff'
import { EmployeeShift } from "./EmployeeShift"
import { PaymentInfo } from "./PaymentInfo"
import { Roles } from "./Roles"
import { MyDepartmentAttendance } from "./MyDepartmentAttendance"
import { ReportingToEmployee } from "./ReportingToEmployee"
import { ResignedDepartmentEmployees } from "./ResignedDepartmentEmployees"
import {RCMAttendance} from "./RCMAttendance"
import { ProvidentFundInfo } from "./ProvidentFundInfo"
import { ESIInfo } from "./ESIInfo"
const CodingMap = {
    //LMS
    ActiveEmployees: AllActiveEmployees,
    Attendance: Attendance,
    ContactDetails: EmployeeContactDetails,
    EmployeeAddress: EmployeeAddress,
    LeaveBalances: LeaveBalanced,
    MyTeamAttendance: MyTeamAttendance,
    MyDepartmentAttendance : MyDepartmentAttendance,
    MyTeamLeaveBalance: MyTeamLeaveBalance,
    ResignedEmployees: ResignedDepartmentEmployees,
    // ResignedDepartmentEmployees: ResignedDepartmentEmployees,
    RCMAttendance : RCMAttendance,
    EmployeeWeekOff: EmployeeWeekOff,
    EmployeeShift: EmployeeShift,
    //: ReportingToEmployee,
    //PayRoll
    //Arrears: Arrears,
    Arrears : Roles,
    EPF: EmployeeEPF,
    ESI: EmployeeESI,
    IncentivePay: IncentivePay,
    IncomeTax: IncomeTax,
    ProfessionalTax: ProfessionalTax,
    PaymentInfo:PaymentInfo,
    ProvidentFundInfo:ProvidentFundInfo,
    ESIInfo: ESIInfo,
    //Page
    //Roles:Roles,


}
export default CodingMap