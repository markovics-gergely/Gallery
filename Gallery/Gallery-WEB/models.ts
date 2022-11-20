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

export interface ProfileViewModel {
    userName: string;
    name: string;
    email: string;
}

export interface FullProfileViewModel {
    userName: string;
    firstName: string;
    lastName: string;
    email: string;
}

export interface ProfileAlbumViewModel {
    id: string;
    name: string;
    countOfPictures: number;
    coverUrl: string;
}

export interface AlbumViewModel {
    id?: string;
    name?: string;
    isPrivate?: boolean;
    isFavorite?: boolean;
    likeCount?: number;
    creator?: UserNameViewModel;
    pictureUrls?: string[];
}

export interface AlbumDetailViewModel {
    id?: string;
    name?: string;
    creator?: UserNameViewModel;
    isPrivate?: boolean;
    isFavorite?: boolean;
    likeCount?: number;
    pictureUrls?: string[];
}

export interface PagerModel {
    page: number;
    pageSize: number;
}

export interface UserNameViewModel {
    id: string;
    userName: string;
}

export interface PagerList<T> {
    total: number;
    values: T[];
}

export interface CreateAlbumDTO {
    name: string,
    isPrivate: boolean,
    pictures: File[]
}

export interface EditFavoritesDTO {
    albumIds: string[];
}

export interface AddAlbumPicturesDTO {
    pictures: File[];
}

export interface RemoveAlbumPicturesDTO {
    PictureIds: string[];
}