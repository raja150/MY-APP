import APIService from "../apiservice";
const apiUrl ="Leave/Holidays"

class HolidaysService{
    async getHolidays() {
        return await APIService.getAsync(`${apiUrl}/GetList`)
    }
    UpComingHolidaysPaginate() {
        return { searchAPI: `${apiUrl}/Future` };
    }
    PastHolidaysPaginate() {
        return { searchAPI: `${apiUrl}/Past` };
    }
}
export default new HolidaysService()