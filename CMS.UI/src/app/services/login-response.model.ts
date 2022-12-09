import { UserType } from '../storage/enums/user-type';

export class LoginResponseModel {
  token: string = '';
  fullName: string = '';
  expireDate?: Date;
  userType?: UserType;
}
