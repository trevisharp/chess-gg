import Logo from "~/components/Logo/Logo";

export default function MainLayout({ children }: { children: React.ReactNode })
{
    return (
        <div className="h-screen flex flex-col">
            <header>
                <div className="flex flex-col items-center">
                    <Logo/>
                </div>
            </header>

            <main className="flex-1 flex">
            {children}
            </main>

            <footer className="p-5 text-center">
            Trevisharp© 2026
            </footer>
        </div>
    )
}