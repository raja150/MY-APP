import APIService from '../apiservice';
const apiUrl = 'Leave/Approval/ClientVisit';

class ClientVisitService {

    paginate() {
        return { searchAPI: `${apiUrl}` };
    }
    async getAsync(id) {
        return await APIService.getAsync(`${apiUrl}/${id}`);
    }
    async updateAsync(data) {
        return await APIService.putAsync(`${apiUrl}`, data);
    }
}
export default new ClientVisitService();

