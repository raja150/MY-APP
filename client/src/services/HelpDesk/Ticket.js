import APIService from '../apiservice'
const apiUrl = 'HelpDesk/TicketLog'

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
}
export default new TicketService()