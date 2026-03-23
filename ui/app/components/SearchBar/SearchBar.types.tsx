type SearchBarProps = {
    storageKey: string,
    searchTip: string,
    onSearch: (search: string) => Promise<boolean>
}