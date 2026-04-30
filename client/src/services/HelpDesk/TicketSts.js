import APIService from '../apiservice'
const apiUrl = 'HelpDesk/TicketStatus'
class TicketStsService {
    async getList() {
        return APIService.getAsync(`${apiUrl}/GetList?OrderBy='OrderNo'`)
    }
}
export default new TicketStsService