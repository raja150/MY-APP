import APIService from '../apiservice'
const apiUrl = 'PayRoll/IncentivesPayCut'
class IncentivesPayCutService {
    paginate() {
        return { searchAPI: `${apiUrl}/Paginate` }
    }
    async getById(id) {
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }
    async getIncetivesPayCut(month, year) {
        return await APIService.getAsync(`${apiUrl}/Employee?month=${month}&year=${year}`)
    }
    async getIncetivesPayCutPayRoll(empId, month, year) {
        return await APIService.getAsync(`${apiUrl}/Employee/${empId}?month=${month}&year=${year}`)
    }
    async UpdateAsync(data) {
        if (data.id) {
            return await APIService.putAsync(`${apiUrl}`, data)
        } else {
            return await APIService.postAsync(`${apiUrl}`, data)
        }
    }
    async PayMonths() {
        return await APIService.getAsync(`${apiUrl}/PayMonths`)
    }
}
export default new IncentivesPayCutService()