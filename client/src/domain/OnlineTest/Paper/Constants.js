export const OnlineTestTabs = [

    { id: 0, text: 'Department' },
    { id: 1, text: 'Designation' },
    { id: 2, text: 'Employee' }
]

export const STATUS_VALUE = (value) => {
    return value == 1 ? "Enable" : "Disable"
}
export const LIVE_VALUE = (value) => { return value == 1 ? "Yes" : "No" }

export const TEST_VALUES = [{ value: 'true', text: 'Yes' }, { value: 'false', text: 'No' }]

export const LIVE_STATUS = [{ value: true, text: 'On' }, { value: false, text: "Off" }]

export const STATUS = [{ value: 1, text: 'Enable' }, { value: 2, text: "Disable" }]

export const IS_IN_LIVE = [{ value: 1, text: 'Yes' }, { value: 2, text: "No" }]

export const SHOW_RESULT = [{ value: true, text: 'Allow' }, { value: false, text: "Don't Allow" }]

export const SEARCH_PAPER = 'Search/Paper'