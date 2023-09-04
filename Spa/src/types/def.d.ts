private interface BaseViewModel {
    id: number
    createdOn: Date
}

interface Content extends BaseViewModel {
    title: string
    url: string
    text: string
    summary: string
    imageUrl: string
}

interface Post extends Content {
    views: number
}

interface Category extends Content {
    postCount: number
}

interface CategoryDetail extends Content {
    parentId: number
    parentTtile: string
    posts: Post[]
}

interface PostDetail extends Content {
    views: number
    categories: Category[]
}

interface Paged<T> {
    data: T[],
    pageIndex: number,
    pageSize: number,
    totalCount: number,
    totalPages: number,
    hasPreviousPage: boolean,
    hasNextPage: boolean
}

export type { Post, Category, CategoryDetail, PostDetail, Paged }
