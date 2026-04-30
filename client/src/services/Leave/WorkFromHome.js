import APIService from '../apiservice'
const apiUrl = 'Leave/ApplyWFH'
class WorkFromHomeService {
    LMApprovalPaginate() {
        return { searchAPI: `${apiUrl}/Paginate` }
    }
    ApprovalPaginate() {
        return { searchAPI: `${apiUrl}/Approval/Paginate` }
    }
    SelfServicePaginate() {
        return { searchAPI: `${apiUrl}/SelfServiceSearch` }
    }
    async getById(id) {
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async getPastFutureWFH(employeeid, fromDate, toDate) {
        return await APIService.getAsync(`${apiUrl}/LM/${employeeid}?fromDate=${fromDate}&&toDate=${toDate}`)
    }
    async getApprovalById(id) {
        return await APIService.getAsync(`${apiUrl}/Approval/${id}`)
    }
    async approveOrReject(data) {
        return await APIService.putAsync(`${apiUrl}/LM/Approve`, data);
    }
    async rejectAfterApprove(data){
        return await APIService.putAsync(`${apiUrl}/LM/Reject`,data)
    }
    async approveAproveOrReject(data) {
        return await APIService.putAsync(`${apiUrl}/Approve`, data);
    }

    async getList() {
        return await APIService.getAsync(`${apiUrl}/GetList`)
    }
    async UpdateAsync(data) {
        if (data.id) {
            return await APIService.putAsync(`${apiUrl}/SelfService`, data)
        } else {
            return await APIService.postAsync(`${apiUrl}/SelfService`, data)
        }
    }
    async SelfServiceCancelAsync(id) {
        return await APIService.putAsync(`${apiUrl}/SelfService/Cancel/${id}`);
    }
    async UpdateLMAsync(data) {
        if (data.id) {
            return await APIService.putAsync(`${apiUrl}/LeaveManagement`, data)
        } else {
            return await APIService.postAsync(`${apiUrl}/LeaveManagement`, data)
        }
    }
}
export default new WorkFromHomeService()