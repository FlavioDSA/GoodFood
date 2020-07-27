import { observable, computed, action, runInAction, reaction } from 'mobx';
import { IUser, IUserFormValues } from '../models/user';
import { Stores } from './stores';
import agent from '../api/agent';

export default class UserStore {
  stores: Stores;

  constructor(stores: Stores) {
    this.stores = stores;

    reaction(
      () => this.token,
      token => {
        if (token) {
          window.localStorage.setItem('jwt', token);
        } else {
          window.localStorage.removeItem('jwt')
        }
      }
    )

    reaction(
      () => this.user,
      user => {
        if (user) {
          window.localStorage.setItem('user', JSON.stringify(user));
        } else {
          window.localStorage.removeItem('user')
        }
      }
    )
  }

  @observable user: IUser | null = JSON.parse(window.localStorage.getItem('user')!);
  @observable token: string | null = window.localStorage.getItem('jwt');

  @computed get isLoggedIn() {
    return !!this.token;
  }

  @action loginAsync = async (values: IUserFormValues) => {
    try {
      const user = await agent.User.login(values);
      runInAction(() => {
        this.user = user;
        this.setToken(user.token);
      });
    } catch (error) {
      throw error;
    }
  };

  @action setToken = (token: string | null) => {
    this.token = token;
  }

  @action setUser = (user: IUser | null) => {
    this.user = user;
  }

  // @action register = async (values: IUserFormValues) => {
  //   try {
  //     const user = await agent.User.register(values);
  //     this.rootStore.commonStore.setToken(user.token);
  //     this.rootStore.modalStore.closeModal();
  //     history.push('/activities')
  //   } catch (error) {
  //     throw error;
  //   }
  // }

  // @action getUser = async () => {
  //   try {
  //     const user = await agent.User.current();
  //     runInAction(() => {
  //       this.user = user;
  //     });
  //   } catch (error) {
  //     console.log(error);
  //   }
  // };

  @action logout = async () => {
    this.setToken(null);
    this.setUser(null);
  };
}
