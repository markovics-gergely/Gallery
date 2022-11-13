export enum Roles {
    Admin,
    Regular
}

export interface LoginUserDTO {
    username: string;
    password: string;
    grant_type?: string;
    client_id?: string;
    client_secret?: string;
    scope?: string;
}

export interface LoginUserResponse {
    access_token: string,
    refresh_token: string,
    detail: string,
    expires_in: number
}

export interface RegisterUserDTO {
    userName: string;
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    confirmedPassword: string;
}