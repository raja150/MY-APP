import apiservice from "services/apiservice"

const apiUrl = 'Organization/UpdateProfile'
class UpdateProfileService {
    async DeleteContact(id){
        return await apiservice.putAsync(`${apiUrl}/DeleteProfileContact/${id}`, {})
    }
    async AddContact(data){
        return await apiservice.putAsync(`${apiUrl}/UpdateProfileContact`, data)
    }
    async PersonalInfoEdit(data){
        return await apiservice.putAsync(`${apiUrl}/UpdateFromProfile`, data)
    }
    async UpdateEmergencyAddress(data){
        return await apiservice.putAsync(`${apiUrl}/UpdateEmergencyAddress`, data)
    }
    async UpdatePermanentAddress(data){
        return await apiservice.putAsync(`${apiUrl}/UpdatePermanentAddress`, data)
    }
    async UpdatePresentAddress(data){
        return await apiservice.putAsync(`${apiUrl}/UpdatePresentAddress`, data)
    }
}
export default new UpdateProfileService()