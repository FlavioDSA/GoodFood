import React, { useContext } from 'react'
import { RouteProps, RouteComponentProps, Route, Redirect } from 'react-router-dom';
import { StoresContext } from '../stores/stores';
import { observer } from 'mobx-react-lite';

interface IProps extends RouteProps {
    component: React.ComponentType<RouteComponentProps<any>>
}

const PrivateRoute: React.FC<IProps> = ({component: Component, ...rest}) => {
    const stores = useContext(StoresContext);
    const {isLoggedIn} = stores.userStore;
    return (
        <Route 
            {...rest}
            render={(props) => isLoggedIn ? <Component {...props}/> : <Redirect to='/login' />}
        />
    )
}

export default observer(PrivateRoute)
