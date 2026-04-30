import APIService from '../apiservice'
const apiUrl = 'Organization/WeekOffDays'
class WeekOffDaysService {

    async getWeekOffDaysList() {
        return await APIService.getAsync(`${apiUrl}/GetList`)
    }
    async paginate() {
        return await APIService.getAsync(`${apiUrl}/paginate`)
    }

    async UpdateAsync(data) {
        return await APIService.putAsync(`${apiUrl}/WeekOffDaysSetup`, data)
    }
    
    async GetWeekOffAllocationById(id) {
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }

}
export default new WeekOffDaysService()