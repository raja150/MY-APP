export const TemplateType = [{ value: 1, text: 'Basic' }, { value: 2, text: 'HRA' }, { value: 3, text: 'Loyalty' }]

export function EmptyItemDetails() {
  return {
    componentId: '',
    type: 2,
    percentage: '',
    percentOn: 1,
    monthly: 0,
    annually: 0,
    isDeleted: false,
    componentName: '',
    percentOnCompName: '',
    percentOnCompId: '',
    fromTemplate: false,
  };
}
export function EmptyDeductionDetails() {
  return {
    deductionId: '',
    monthly: 0,
    fromTemplate: false,
    isDeleted: false,
  }

}
export function InitialVals() {
  return {
    templateId: '',
    employeeNo: '',
    annually: 0,
    monthly: 0,
    ctc: '',
    templateComponents: [EmptyItemDetails()],
    salaryDeductions:[EmptyDeductionDetails()]
  }
}
export const Gender = { 0: '', 1: 'Male', 2: 'Female' }