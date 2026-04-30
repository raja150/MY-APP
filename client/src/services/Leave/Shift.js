import APIService from '../apiservice'
const apiUrl = 'Leave/Shift'
class ShiftService {
    async getList() {
        return await APIService.getAsync(`${apiUrl}/GetList`)
    }
}
export default new ShiftService()