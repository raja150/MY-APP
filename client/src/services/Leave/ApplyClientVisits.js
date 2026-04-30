import APIService from '../apiservice'
const apiUrl = 'Leave/ApplyClientVisits'
class ApplyClientVisitsService {
    paginate() {
        return { searchAPI: `${apiUrl}/Paginate` }
    }
    LMApprovalPaginate() {
        return { searchAPI: `${apiUrl}/LMPaginate` }
    }
    async getById(id) {
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async getList() {
        return await APIService.getAsync(`${apiUrl}/GetList`)
    }
    async UpdateAsync(data) {
        if (data.id) {
            return await APIService.putAsync(`${apiUrl}`, data)
        } else {
            return await APIService.postAsync(`${apiUrl}`, data)
        }
    }
    async SelfServiceCancelAsync(id) {
        return await APIService.putAsync(`${apiUrl}/SelfService/Cancel/${id}`);
    }

    //#region  LeaveManagement
    async UpdateLMAsync(data) {
        return await APIService.postAsync(`${apiUrl}/LeaveManagement`, data)
    }
    async approveOrReject(data) {
        return await APIService.putAsync(`${apiUrl}/LM/Approve`, data);
    }
    async getPastFutureVisits(employeeid, fromDate, toDate) {
        return await APIService.getAsync(`${apiUrl}/LM/${employeeid}?fromDate=${fromDate}&&toDate=${toDate}`)
    }
    //#endregion

    //#region  Approve
    async approveAproveOrReject(data) {
        return await APIService.putAsync(`${apiUrl}/Approve`, data);
    }
    ApprovalPaginate() {
        return { searchAPI: `${apiUrl}/Approval/Paginate` }
    }
    //#endregion
}
export default new ApplyClientVisitsService()