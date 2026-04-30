import APIService from '../apiservice'

const apiUrl = 'PayRoll/PayRun'
class PayRunService {
    async PayRunHold(id) {
        return await APIService.putAsync(`${apiUrl}/Hold/${id}`)
    }
    async PayRunRelease(id) {
        return await APIService.putAsync(`${apiUrl}/Release/${id}`)
    }
    async PayRunById(id) {
        return await APIService.putAsync(`${apiUrl}/${id}`)
    }
    async deletePayRun(id) {
        return await APIService.deleteAsync(`${apiUrl}/${id}`)
    }
}
export default new PayRunService