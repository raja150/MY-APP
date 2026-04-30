import APIService from '../apiservice'
const apiUrl = 'SelfService/Ticket'

class TicketService {
    paginate() {
        return { searchAPI: `${apiUrl}/Paginate` }
    }
    async getById(id) {
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async PostAsync(data) {
        return await APIService.postFormDataAsync(`${apiUrl}`, data)
    }
    async PutAsync(data) {
        return await APIService.putFormDataAsync(`${apiUrl}`, data)
    }
    async Download(id, fileName) {
        return await APIService.getBlobAsync(`${apiUrl}/Download/${id}/${fileName}`)
    }
    async UserResponse(data) {
        return await APIService.postAsync(`${apiUrl}/UserResponse`, data)
    }
    async UpdateResponse(data) {
        return await APIService.putAsync(`${apiUrl}/Response`, data)
    }
}
export default new TicketService()