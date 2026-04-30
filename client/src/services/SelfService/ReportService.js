import APIService from '../apiservice';

class ReportService {
    async getLeavesInfo() {
        return await APIService.getAsync(`SelfService/Report/Leaves`)
    }
    async getEmployeeLeavesInfo() {
        return await APIService.getAsync(`SelfService/Report/EmployeeLeaves`)
    }
    async employeeProfile() {
        return await APIService.getAsync(`SelfService/Report/EmployeeProfile`)
    }
    //Display leave balance of an employee
    async getLeaveBalance() {
        return await APIService.getAsync(`SelfService/Report/LeaveBalance`)
    }
    //To display Leave Balance Details in SelfService LeaveBalance screen
    getLeaveBalanceDetailsPaginate(){
        return {searchAPI: `LMSReport/LeaveBalanceDetails`}
    }
}
export default new ReportService()