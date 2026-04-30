import APIService from '../apiservice'
const apiUrl = 'LM/ApplyLeave'

class ApplyLeavesService {

    SelfServicePaginate() {
        return { searchAPI: `${apiUrl}/SelfService/Paginate` }
    }
    RequestDetailsPaginate(){
        return {searchAPI: `${apiUrl}/LeaveDetails`}
    }
    async getSelfServiceById(id) {
        return await APIService.getAsync(`${apiUrl}/SelfService/${id}`)
    }

    async SelfServiceUpdateAsync(data) {
        if (data.id) {
            return await APIService.putAsync(`${apiUrl}/SelfService`, data)
        } else {
            return await APIService.postAsync(`${apiUrl}/SelfService`, data)
        }
    }

    async SelfServiceCancelAsync(id) {
        return await APIService.putAsync(`${apiUrl}/SelfService/Cancel/${id}`);
    }
    async GetLeaveBalanceByLeaveType(leaveTypeId, fromDate, toDate) {
        return await APIService.getAsync(`${apiUrl}/LeaveBalance/LeaveType/${leaveTypeId}/${fromDate}/${toDate}`);
    }
    async GetEmployeeLeaveType() {
        return await APIService.getAsync(`${apiUrl}/EmpLeaveType`);
    }
    async GetLeaveBalanceByEmpLeaveType(leaveTypeId, empId,fromDate,toDate) {
        return await APIService.getAsync(`${apiUrl}/LeaveBalance/EmpLeaveType/${leaveTypeId}/${empId}/${fromDate}/${toDate}`);
    }
    async GetSearchedEmpLeaveTypes(empId) {
        return await APIService.getAsync(`${apiUrl}/EmpLeaveType/${empId}`)
    }
    //Apply leave by HR behalf of an employee
    async LeavePostAsync(data) {
        return await APIService.postAsync(`${apiUrl}/Leave`, data)
    }

    //Display leave info to approve or reject a leave request
    async getLeaveById(id) {
        return await APIService.getAsync(`${apiUrl}/Leave/${id}`)
    }
    async GetWeekLeaves(employeeId, fromDate, toDate) {
        return await APIService.getAsync(`${apiUrl}/Leaves/${employeeId}?fromDate=${fromDate}&&toDate=${toDate}`)
    }
    //HR approve or reject leave
    async approveOrReject(data) {
        return await APIService.putAsync(`${apiUrl}/Leave/Approve`, data);
    }
    //HR Reject Leaves after approve
    async rejectAfterApprove(data) {
        return await APIService.putAsync(`${apiUrl}/Leave/Reject`, data);
    }
    //Display leaves in calender for HR
    async GetLeavesByMonth(month) {
        return await APIService.getAsync(`${apiUrl}/Leave/MonthLeaves?month=${month}`)
    }
    async GetLeavesBetweenDates(fromDate, toDate) {
        return await APIService.getAsync(`${apiUrl}/Leave/BetweenDates?fromDate=${fromDate}&&toDate=${toDate}`)
    }

    //HR Leave pagination
    LMApprovalPaginate() {
        return { searchAPI: `${apiUrl}/Leave/Paginate` };
    }

    //Get leaves pagination for the approver
    ApprovalPaginate() {
        return { searchAPI: `${apiUrl}/Approval/Paginate` };
    }
    //Get leave information for the approver
    async getApprovalByIdAsync(id) {
        return await APIService.getAsync(`${apiUrl}/Approval/${id}`);
    }
    async updateAsync(data) {
        return await APIService.putAsync(`${apiUrl}`, data);
    }

    //the approver approve or reject leave
    async approverApproveOrReject(data) {
        return await APIService.putAsync(`${apiUrl}/Approval/Approve`, data);
    }
}
export default new ApplyLeavesService()