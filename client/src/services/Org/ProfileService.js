import apiservice from "services/apiservice"

const apiUrl = 'Organization/Employee'
class ProfileService {
  
    async getContact() {
        return await apiservice.getAsync(`${apiUrl}/ContactDetails`)
    }
    async getEmployeeDetails() {
        return await apiservice.getAsync(`${apiUrl}/EmpDetails`)
    }
    async getPresentAddress() {
        return await apiservice.getAsync(`${apiUrl}/PresentAddress`)
    }
    async getPermanentAddress() {
        return await apiservice.getAsync(`${apiUrl}/PermanentAddress`)
    }
    async getEmergencyAddress() {
        return await apiservice.getAsync(`${apiUrl}/EmergencyAddress`)
    }
}
export default new ProfileService()