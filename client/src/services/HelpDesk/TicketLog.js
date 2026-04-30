import APIService from '../apiservice'
const apiUrl = 'HelpDesk/TicketLog'
class TicketLogService {

    async postAsync(data) {
        return await APIService.postAsync(`${apiUrl}`, data)
    }
    async getTicketLogResponse(id) {
        return await APIService.getAsync(`${apiUrl}/Response/${id}`)
    }
    paginate() {
        return { searchAPI: `${apiUrl}/DeptTickets` }
    }
    async DeptTransfer(data) {
        return await APIService.putAsync(`${apiUrl}/Transfer`, data)
    }
    async ReAssign(data) {
        return await APIService.putAsync(`${apiUrl}/ReAssign`, data)
    }
    async getRespById(id) {
        return await APIService.getAsync(`${apiUrl}/ResponseInfo/${id}`)
    }
    async updateResponse(data) {
        return await APIService.putAsync(`${apiUrl}/Response`, data)
    }
    paginate() {
        return { searchAPI: `${apiUrl}/Paginate` }
    }
    async getById(id) {
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async PostAsync(data) {
        return await APIService.postFormDataAsync(`${apiUrl}/New`, data)
    }
    async PutAsync(data) {
        return await APIService.putFormDataAsync(`${apiUrl}`, data)
    }
}
export default new TicketLogService()