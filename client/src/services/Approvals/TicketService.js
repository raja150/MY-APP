import APIService from '../apiservice';
const apiUrl = 'Approval/ApprovalRaiseTicket';
class TicketService{

    paginate(){
        return { searchAPI: `${apiUrl}` };
    }
    async updateAsync(data){
        if(data.id){
            return await APIService.putAsync('Approval/ApprovalRaiseTicket', data)
        }
    }
}
export default new TicketService();