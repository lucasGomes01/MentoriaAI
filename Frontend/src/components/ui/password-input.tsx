import * as React from "react"
import { Eye, EyeOff } from "lucide-react"
import { cn } from "@/lib/utils"

type Props = React.InputHTMLAttributes<HTMLInputElement> & {
  label?: string
  containerClassName?: string
  labelClassName?: string
}

const PasswordInput = React.forwardRef<HTMLInputElement, Props>(
  ({ label, id, className, containerClassName, labelClassName, ...props }, ref) => {
    const [show, setShow] = React.useState(false)
    return (
      <div className={cn(containerClassName)}>
        {label ? (
          <label htmlFor={id} className={cn("block text-sm text-gray-700", labelClassName)}>
            {label}
          </label>
        ) : null}
        <div className={cn("relative mt-1")}>
          <input
            id={id}
            ref={ref}
            type={show ? "text" : "password"}
            className={cn(
              "w-full rounded-xl border border-gray-200 px-3 py-2 pr-20 text-sm outline-none focus:ring-2 focus:ring-brand-500",
              className
            )}
            {...props}
          />
          <button
            type="button"
            onClick={() => setShow((v) => !v)}
            aria-label={show ? "Ocultar senha" : "Mostrar senha"}
            aria-pressed={show}
            title={show ? "Ocultar senha" : "Mostrar senha"}
            className={cn(
              "absolute right-2 top-1/2 -translate-y-1/2 inline-flex h-8 w-8 items-center justify-center rounded-full border border-brand-300 bg-brand-100 text-brand-700 shadow-sm hover:bg-brand-200 focus:outline-none focus:ring-2 focus:ring-brand-500"
            )}
          >
            {show ? <EyeOff className="h-4 w-4" /> : <Eye className="h-4 w-4" />}
          </button>
        </div>
      </div>
    )
  }
)
PasswordInput.displayName = "PasswordInput"

export { PasswordInput }
