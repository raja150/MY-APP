import React, { useState } from 'react'
import { Row } from 'reactstrap'
import AddTestEmployee from './Employee/index'
import AddTestDesignation from './Designation/index'
import AddTestDepartment from './Department/index'
import DepartmentList from "./Department/List";
import DesignationList from "./Designation/List";
import EmployeeList from "./Employee/List";
import * as compare from 'utils/Compare';
import Tabs, { TabPane } from 'rc-tabs';
import ScrollableInkTabBar from 'rc-tabs/lib/ScrollableInkTabBar';
import TabContent from 'rc-tabs/lib/TabContent';
import { OnlineTestTabs } from './Constants'

export default function TestTabs(props) {

    const [empAdd, setEmpAdd] = useState(false)
    const [deptAdd, setDeptAdd] = useState(false)
    const [designAdd, setDesignAdd] = useState(false)

    const { currentId, rid } = props

    const handleAddEmp = async () => {
        setEmpAdd(!empAdd)
    }
    const handleAddDept = () => {
        setDeptAdd(!deptAdd)
    }
    const handleAddDesign = () => {
        setDesignAdd(!designAdd)
    }

    return (
        <>
            <Row>
                <Tabs defaultActiveKey="0"
                    renderTabBar={() => <ScrollableInkTabBar />}
                    renderTabContent={() => <TabContent />}
                >
                    {
                        OnlineTestTabs.length > 0 && OnlineTestTabs.map((tab, key) => {
                            return (
                                <TabPane tab={tab.text} key={key}>

                                    {
                                        compare.isEqual(key, 0) && deptAdd ? <AddTestDepartment refId={rid ? rid : currentId} {...props} handleAddDept={handleAddDept} /> :
                                            compare.isEqual(key, 0) && <DepartmentList refId={rid ? rid : currentId} {...props} handleAddDept={handleAddDept} />

                                    }
                                    {
                                        compare.isEqual(key, 1) && designAdd ? <AddTestDesignation refId={rid ? rid : currentId} {...props} handleAddDesign={handleAddDesign} /> :
                                            compare.isEqual(key, 1) && <DesignationList refId={rid ? rid : currentId} {...props} handleAddDesign={handleAddDesign} />

                                    }
                                    {
                                        compare.isEqual(key, 2) && empAdd ? <AddTestEmployee refId={rid ? rid : currentId} {...props} handleAddEmp={handleAddEmp} />
                                            : compare.isEqual(key, 2) && <EmployeeList refId={rid ? rid : currentId} {...props} handleAddEmp={handleAddEmp} />
                                    }
                                </TabPane>
                            )
                        })
                    }
                </Tabs>
            </Row>
        </>
    )
}
